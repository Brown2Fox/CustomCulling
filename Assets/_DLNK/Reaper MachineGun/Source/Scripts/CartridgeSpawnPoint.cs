// 
// CartridgeSpawnPoint.cs
//  
// Author:
//       Jose A. Milan <jose@jamilan.net>
// 
// Copyright (c) 2013 Jose A. Milan - 2013
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;

// Mark the owner object as a Cartridge Spawn Point
public class CartridgeSpawnPoint : MonoBehaviour
{
	public float MinRandomForce = 0.0001f;
	public float MaxRandomForce = 0.0005f;
	public float ExpulsionForce = 0.00035f;
	
	private GameObject _cartridgePrefab;
	
	public void Setup(GameObject cartridgePrefab)
	{
		_cartridgePrefab = cartridgePrefab;
	}
	
	public void Spawn()
	{
		var go = Instantiate(_cartridgePrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		go.GetComponent<Rigidbody>().AddRelativeForce(ExpulsionForce, Random.Range(MinRandomForce, MaxRandomForce), 0, ForceMode.VelocityChange);
	}
}
