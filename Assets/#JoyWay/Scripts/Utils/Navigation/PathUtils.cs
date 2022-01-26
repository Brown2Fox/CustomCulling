//
// Copyright (c) 2009-2010 Mikko Mononen memon@inside.org
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//

// The original source code has been modified by Unity Technologies.

using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Experimental.AI;

[Flags]
public enum StraightPathFlags {
	Start = 0x01,            // The vertex is the start position.
	End = 0x02,              // The vertex is the end position.
	OffMeshConnection = 0x04 // The vertex is start of an off-mesh link.
}

public class PathUtils {
	public static float Perp2D(Vector3 u, Vector3 v) {
		return u.z * v.x - u.x * v.z;
	}

	public static void Swap(ref Vector3 a, ref Vector3 b) {
		Vector3 temp = a;
		a = b;
		b = temp;
	}

	// Retrace portals between corners and register if type of polygon changes
	public static int RetracePortals(NavMeshQuery query, int startIndex, int endIndex, NativeSlice<PolygonId> path, int n, Vector3 termPos
								   , ref NativeArray<NavMeshLocation> straightPath, int maxStraightPath) {
#if DEBUG_CROWDSYSTEM_ASSERTS
        Assert.IsTrue(n < maxStraightPath);
        Assert.IsTrue(startIndex <= endIndex);
#endif
		Vector3 startPos = straightPath[n - 1].position;


		for (int k = startIndex; k < endIndex; ++k) {
			Vector3 l, r;

			bool status = query.GetPortalPoints(path[k], path[k + 1], out l, out r);

#if DEBUG_CROWDSYSTEM_ASSERTS
                Assert.IsTrue(status); // Expect path elements k, k+1 to be verified
#endif

			float3 cpa1, cpa2;

			if (straightPath[n - 1].position != termPos) {
				startPos = straightPath[n - 1].position;
			}

			GeometryUtils.SegmentSegmentCPA(out cpa1, out cpa2, l, r, startPos, termPos);

			if (query.GetPolygonType(path[k]) == NavMeshPolyTypes.OffMeshConnection) {
				Vector3 point = cpa1;
				Vector3 planeN = Vector3.Cross(l - r, l - point);
				Vector3 planefwd = Vector3.Cross(l - r, planeN);


				Vector3 mid = (Vector3) cpa1 - straightPath[n - 1].position;

				Vector3 fwd = Vector3.Project(mid, planefwd);

				cpa1 = straightPath[n - 1].position + fwd;
			}


			if (query.GetPolygonType(path[k + 1]) != NavMeshPolyTypes.OffMeshConnection) {
				straightPath[n] = query.CreateLocation(cpa1, path[k + 1]);
			} else {
				straightPath[n] = query.CreateLocation(cpa1, path[k]);
			}

			if (++n == maxStraightPath) {
				return maxStraightPath;
			}
		}

		return n;
	}

	public static PathQueryStatus FindStraightPath(NavMeshQuery query, Vector3 startPos, Vector3 endPos
												 , NativeSlice<PolygonId> path, int pathSize
												 , ref NativeArray<NavMeshLocation> straightPath
												 , ref NativeArray<float> vertexSide
												 , ref int straightPathCount
												 , int maxStraightPath) {
#if DEBUG_CROWDSYSTEM_ASSERTS
        Assert.IsTrue(pathSize > 0, "FindStraightPath: The path cannot be empty");
        Assert.IsTrue(path.Length >= pathSize, "FindStraightPath: The array of path polygons must fit at least the size specified");
        Assert.IsTrue(maxStraightPath > 1, "FindStraightPath: At least two corners need to be returned, the start and end");
        Assert.IsTrue(straightPath.Length >= maxStraightPath, "FindStraightPath: The array of returned corners cannot be smaller than the desired maximum corner count");
        Assert.IsTrue(straightPathFlags.Length >= straightPath.Length, "FindStraightPath: The array of returned flags must not be smaller than the array of returned corners");
#endif

		if (!query.IsValid(path[0])) {
			straightPath[0] = new NavMeshLocation(); // empty terminator
			return PathQueryStatus.Failure;          // | PathQueryStatus.InvalidParam;
		}

		straightPath[0] = query.CreateLocation(startPos, path[0]);

		int apexIndex = 0;
		int n = 1;

		if (pathSize > 1) {
			Matrix4x4 startPolyWorldToLocal = query.PolygonWorldToLocalMatrix(path[0]);

			Vector3 apex = startPolyWorldToLocal.MultiplyPoint(startPos);
			Vector3 left = new Vector3(0, 0, 0); // Vector3.zero accesses a static readonly which does not work in burst yet
			Vector3 right = new Vector3(0, 0, 0);
			int leftIndex = -1;
			int rightIndex = -1;

			for (int i = 1; i <= pathSize; ++i) {
				Matrix4x4 polyWorldToLocal = query.PolygonWorldToLocalMatrix(path[apexIndex]);

				Vector3 vl, vr;
				if (i == pathSize) {
					vl = vr = polyWorldToLocal.MultiplyPoint(endPos);
				} else {
					bool success = query.GetPortalPoints(path[i - 1], path[i], out vl, out vr);
					if (!success) {
						return PathQueryStatus.Failure; // | PathQueryStatus.InvalidParam;
					}

#if DEBUG_CROWDSYSTEM_ASSERTS
						Assert.IsTrue(query.IsValid(path[i - 1]));
						Assert.IsTrue(query.IsValid(path[i]));
#endif

					vl = polyWorldToLocal.MultiplyPoint(vl);
					vr = polyWorldToLocal.MultiplyPoint(vr);
				}

				vl -= apex;
				vr -= apex;

				// Ensure left/right ordering
				if (Perp2D(vl, vr) < 0)
					Swap(ref vl, ref vr);

				//Terminate funnel becausae of link
				if (query.GetPolygonType(path[i]) == NavMeshPolyTypes.OffMeshConnection) {
					//|| query.GetPolygonType(path[i + 1]) == NavMeshPolyTypes.OffMeshConnection) {
					Matrix4x4 polyLocalToWorld = query.PolygonLocalToWorldMatrix(path[apexIndex]);

					Vector3 point = new Vector3(0, 0, 0);

					if (vl.sqrMagnitude < vr.sqrMagnitude) {
						if (GeometryUtils.TriangleAngleProportions(point, vr, vl) >= 0) {
							point = vl;
						}
					} else {
						if (GeometryUtils.TriangleAngleProportions(point, vl, vr) >= 0) {
							point = vr;
						}
					}


					Vector3 termPos = polyLocalToWorld.MultiplyPoint(apex + point);

					n = RetracePortals(query, apexIndex, i - 1, path, n, termPos, ref straightPath, maxStraightPath);
					n = RetracePortals(query, i - 1, i, path, n, termPos, ref straightPath, maxStraightPath);
					n = RetracePortals(query, i, i + 1, path, n, termPos, ref straightPath, maxStraightPath);
					if (vertexSide.Length > 0) {
						vertexSide[n - 1] = 69;
					}

					//Debug.Log("LINK");

					if (n == maxStraightPath) {
						straightPathCount = n;
						return PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
					}

					apex = polyWorldToLocal.MultiplyPoint(termPos);
					left.Set(0, 0, 0);
					right.Set(0, 0, 0);
					apexIndex = ++i;
					continue;
				}

				// Terminate funnel by turning
				if (Perp2D(left, vr) < 0) {
					Matrix4x4 polyLocalToWorld = query.PolygonLocalToWorldMatrix(path[apexIndex]);
					Vector3 termPos = polyLocalToWorld.MultiplyPoint(apex + left);

					n = RetracePortals(query, apexIndex, leftIndex, path, n, termPos, ref straightPath, maxStraightPath);
					if (vertexSide.Length > 0) {
						vertexSide[n - 1] = -1;
					}

					//Debug.Log("LEFT");

					if (n == maxStraightPath) {
						straightPathCount = n;
						return PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
					}

					apex = polyWorldToLocal.MultiplyPoint(termPos);
					left.Set(0, 0, 0);
					right.Set(0, 0, 0);
					i = apexIndex = leftIndex;
					continue;
				}

				if (Perp2D(right, vl) > 0) {
					Matrix4x4 polyLocalToWorld = query.PolygonLocalToWorldMatrix(path[apexIndex]);
					Vector3 termPos = polyLocalToWorld.MultiplyPoint(apex + right);

					n = RetracePortals(query, apexIndex, rightIndex, path, n, termPos, ref straightPath, maxStraightPath);
					if (vertexSide.Length > 0) {
						vertexSide[n - 1] = 1;
					}

					//Debug.Log("RIGHT");

					if (n == maxStraightPath) {
						straightPathCount = n;
						return PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
					}

					apex = polyWorldToLocal.MultiplyPoint(termPos);
					left.Set(0, 0, 0);
					right.Set(0, 0, 0);
					i = apexIndex = rightIndex;
					continue;
				}

				// Narrow funnel
				if (Perp2D(left, vl) >= 0) {
					left = vl;
					leftIndex = i;
				}

				if (Perp2D(right, vr) <= 0) {
					right = vr;
					rightIndex = i;
				}
			}
		}

		// Remove the the next to last if duplicate point - e.g. start and end positions are the same
		// (in which case we have get a single point)
		if (n > 0 && (straightPath[n - 1].position == endPos))
			n--;

		n = RetracePortals(query, apexIndex, pathSize - 1, path, n, endPos, ref straightPath, maxStraightPath);
		if (vertexSide.Length > 0) {
			vertexSide[n - 1] = 0;
		}

		if (n == maxStraightPath) {
			straightPathCount = n;
			return PathQueryStatus.Success; // | PathQueryStatus.BufferTooSmall;
		}


		straightPath[n] = query.CreateLocation(endPos, path[pathSize - 1]);


		straightPathCount = n + 1;
		return PathQueryStatus.Success;
	}
}