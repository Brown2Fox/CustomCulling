My email is "kripto289@gmail.com"
You can contact me for any questions.

My English is not very good, and if there are any translation errors, you can let me know :)

This simple tutorial only for Default Legacy rendering.
Pack includes prefabs of main effects + prefabs of collision effects (Assets\KriptoFX\Realistic Effects Pack v1\Prefabs\).

Supported platforms: PC/Consoles/VR/Mobiles
All effects tested on Oculus Rift CV1 with single and dual mode rendering and work correctly.
*If you use URP rendering then you must import HDRP or URP patches "\Assets\KriptoFX\Realistic Effects Pack v4\HDRP and URP patches"

------------------------------------------------------------------------------------------------------------------------------------------
NOTE: For correct effects working you must:

1) You need activate HDR rendering. Edit -> ProjectSettings -> Graphics -> select current build target -> uncheck "use default" for tier1, tier2, tier3 -> set "Use HDR = true"
2) Enable HDR rendering in the current camera. MainCamera -> "AllowHDR = true"
f you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "post effect msaa".

3) Add postprocessing stack package to project. Window -> Package Manager -> PostProcessing -> Instal
4) MainCamera -> AddComponent -> "Post Processing Layer". For "Layer" you should set custom layer (for example UI, or Postprocessing)
5) Create empty Gameobject and set custom layer as in the camera processing layer (for example UI). Gameobject -> AddComponent -> "Post Process Volume".
Add included postprocessing profile to "Post Process Volume" "\Assets\KriptoFX\Realistic Effects Pack v1\PostProcess Profile.asset"


------------------------------------------------------------------------------------------------------------------------------------------
Using effects:

Just drag and drop prefab of effect on scene and use that :)
If you want use effects in runtime, use follow code:

"Instantiate(prefabEffect, position, rotation);"

Using projectile collision event:
void Start ()
{
	var tm = GetComponentInChildren<RFX1_TransformMotion>(true);
	if (tm!=null) tm.CollisionEnter += Tm_CollisionEnter;
}

private void Tm_CollisionEnter(object sender, RFX1_TransformMotion.RFX1_CollisionInfo e)
{
        Debug.Log(e.Hit.transform.name); //will print collided object name to the console.
}

Using shield interaction:
You need add script "RFX1_ShieldInteraction" on projectiles which should react on shields.

------------------------------------------------------------------------------------------------------------------------------------------
Effect modification:

For scaling just change "transform" scale of effect.
All effects includes helpers scripts (collision behaviour, light/shader animation etc) for work out of box.
Also you can add additional scripts for easy change of base effects settings. Just add follow scripts to prefab of effect.

RFX1_EffectSettingColor - for change color of effect (uses HUE color). Can be added on any effect.
RFX1_EffectSettingProjectile - for change projectile fly distance, speed and collided layers.
RFX1_EffectSettingVisible - for change visible status of effect using smooth fading by time.
RFX1_Target - for homing move to target.

