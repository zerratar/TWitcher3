/*
Witcher 3 console expansions
made by: SkacikPL
extra dialog and notifications by: Zerratar
*/

exec function dialog(title : string, message : string): void  {
	theGame.GetGuiManager().ShowUserDialogAdv(0, title, message, false, UDB_Ok);
}
exec function n(messageText : string, optional duration : float, optional queue : bool ): void {
	theGame.GetGuiManager().ShowNotification(messageText, duration, queue);
}
exec function notify(messageText : string, optional duration : float, optional queue : bool ): void {
	theGame.GetGuiManager().ShowNotification(messageText, duration, queue);
}

exec function fall(optional dist : float): void {
	var playerpos	: Vector;
	
	playerpos = thePlayer.GetWorldPosition();
	if (!dist)
		dist = 6;
	if (dist > 7)
		dist = 7;
	if (dist < 0)
		dist = 0;		
	thePlayer.Teleport(playerpos + Vector(0,0,dist));
}

//Phase
exec function phase(dist : float)
{
var playerheading : float; 
var playerpos	: Vector;

playerheading = thePlayer.GetHeading();
playerpos = thePlayer.GetWorldPosition();

thePlayer.Teleport(playerpos + VecFromHeading(playerheading) * dist);
}
//Unlock door within radius
exec function skeletonkey()
{
		var l_actors	: array< CGameplayEntity >;
		var i :int;
		var ent			: CEntity;
		
		FindGameplayEntitiesInRange( l_actors, thePlayer, 20, 100 );
			for	( i = 0; i < l_actors.Size(); i+= 1 )
				{
					ent = (CEntity) l_actors[i];
					if(((W3NewDoor)ent))
					{
					GetWitcherPlayer().DisplayHudMessage("Unlocked");
					((W3NewDoor)ent).Unlock();
					}
				}
}
//Remove current target from existence
exec function GTFO()
{
var act : CGameplayEntity;
var ent : CEntity;

act = thePlayer.GetDisplayTarget();
ent = (CEntity) act;

ent.Destroy();
}
//Animation control
exec function playcustomanim(val : int,optional animName : name)
{
thePlayer.PlayerStartAction( val,animName );
}
//Quick debug1
exec function gettemplate()
{
		var l_actors	: array< CGameplayEntity >;
		var i :int;
		var ent			: CEntity;
		
		FindGameplayEntitiesInRange( l_actors, thePlayer, 5, 3 );
			for	( i = 0; i < l_actors.Size(); i+= 1 )
				{
					ent = (CEntity) l_actors[i];
					if(ent && ent != thePlayer)
					{
//					GetWitcherPlayer().DisplayHudMessage(ent.GetReadableName());
					theGame.GetGuiManager().ShowUserDialogAdv(0, "Entity Template", ent.GetReadableName(), false, UDB_Ok);
					}
				}
}



//Quick debug2
exec function gettag(optional num : int)
{
		var l_actors	: array< CGameplayEntity >;
		var i :int;
		var ent			: CEntity;
		var tags		:  array<CName>;
		
		if(!num)
			num = 0;
		
		FindGameplayEntitiesInRange( l_actors, thePlayer, 5, 3 );
			for	( i = 2; i < l_actors.Size(); i+= 1 )
				{
					ent = (CEntity) l_actors[i];
					if(ent && ent != thePlayer)
					{
					tags = ((CNode)ent).GetTags();
//					GetWitcherPlayer().DisplayHudMessage(tags[num]);
					theGame.GetGuiManager().ShowUserDialogAdv(0, "Item Tag", tags[num], false, UDB_Ok);
					}
				}
}
//Toggle HUD
exec function thud()
{
var playerhud : CR4ScriptedHud;

playerhud = (CR4ScriptedHud)theGame.GetHud();
playerhud.ForceShow( !playerhud.GetHudFlash().GetVisible(), HVS_System );
}
//Set player scale
exec function setplayerscale(W : float, D : float, H : float)
{
var testcomp : CComponent;

testcomp = thePlayer.GetComponentByClassName('CAnimatedComponent');
testcomp.SetScale(Vector(W,D,H,1));
}
//Set target scale
exec function setscale(W : float, D : float, H : float)
{
var animcomp : CComponent;
var meshcomp : CComponent;
var act : CGameplayEntity;
var ent : CEntity;

act = thePlayer.GetDisplayTarget();
ent = (CEntity) act;

animcomp = ent.GetComponentByClassName('CAnimatedComponent');
meshcomp = ent.GetComponentByClassName('CMeshComponent');

	if(animcomp)
		animcomp.SetScale(Vector(W,D,H,1));
		
	if(meshcomp)
		meshcomp.SetScale(Vector(W,D,H,1));
}
//Enable deleted scene
exec function enabledeletedscene()
{
FactsAdd("q401_cooking_enabled");
}
//Play any sound
exec function playsound(sound : String)
{
theSound.SoundEvent(sound);
}
//Show player position
exec function getpos()
{
theGame.GetGuiManager().ShowUserDialogAdv(0, "Player Position", VecToString(thePlayer.GetWorldPosition()), false, UDB_Ok);
}
//Show player rotation
exec function getrot()
{
var rot : EulerAngles;
rot = thePlayer.GetWorldRotation();
theGame.GetGuiManager().ShowUserDialogAdv(0, "Player Rotation",rot.Pitch + " " + rot.Yaw + " " + rot.Roll , false, UDB_Ok);
}
//Get precise template
exec function gettargettemplate()
{
var act : CGameplayEntity;
var ent : CEntity;

act = thePlayer.GetDisplayTarget();
ent = (CEntity) act;

theGame.GetGuiManager().ShowUserDialogAdv(0, "Entity Template", ent.GetReadableName(), false, UDB_Ok);
}
//Get appearance
exec function getapp()
{
var act : CGameplayEntity;
var ent : CEntity;

act = thePlayer.GetDisplayTarget();
ent = (CEntity) act;

theGame.GetGuiManager().ShowUserDialogAdv(0, "Actor appearance", ((CActor)ent).GetAppearance(), false, UDB_Ok);
}
//Automatic walking
exec function autowalk(val : float)
{
var movcomp : CComponent;

movcomp = thePlayer.GetComponentByClassName('CMovingAgentComponent');
((CMovingAgentComponent)movcomp).ForceSetRelativeMoveSpeed(val);
}
//Look at camera
exec function lookatcam(dur : float)
{
((CActor)thePlayer).DisableLookAt();
((CActor)thePlayer).EnableStaticLookAt(theCamera.GetCameraPosition(),dur);
}
//Add hoods
exec function hood(optional type : int, optional shoulderpiece : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var hoodtemplate : CEntityTemplate;
var shouldertemplate : CEntityTemplate;
var inv : CInventoryComponent;
var witcher : W3PlayerWitcher;
var ids : array<SItemUniqueId>;
var size : int;
var i : int;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(type == 1 || !type)
	hoodtemplate = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\caps\c_01_ma__novigrad_citizen_cloak.w2ent", true);
	
if(type >= 2)
	hoodtemplate = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\caps\c_02_ma__novigrad_cloak.w2ent", true);
	
shouldertemplate = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\items\i_01_ma__novigrad_citizen_cloak.w2ent", true);

if(shoulderpiece)
	((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(shouldertemplate);
	
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(hoodtemplate);
	
witcher = GetWitcherPlayer();
inv = witcher.GetInventory();

ids = inv.GetItemsByCategory( 'hair' );
size = ids.Size();
	
if( size > 0 )
{
		
	for( i = 0; i < size; i+=1 )
	{
		if(inv.IsItemMounted( ids[i] ) )
		inv.DespawnItem(ids[i]);
	}
	
}
	
ids.Clear();

}
//Remove hood
exec function nohood()
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\caps\c_01_ma__novigrad_citizen_cloak.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\items\i_01_ma__novigrad_citizen_cloak.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_cloaks\caps\c_02_ma__novigrad_cloak.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
//Remove hair
exec function nohair()
{
	var inv : CInventoryComponent;
	var witcher : W3PlayerWitcher;
	var ids : array<SItemUniqueId>;
	var size : int;
	var i : int;

	witcher = GetWitcherPlayer();
	inv = witcher.GetInventory();

	ids = inv.GetItemsByCategory( 'hair' );
	size = ids.Size();
	
	if( size > 0 )
	{
		
		for( i = 0; i < size; i+=1 )
		{
			if(inv.IsItemMounted( ids[i] ) )
				inv.DespawnItem(ids[i]);
		}
		
	}
	
	ids.Clear();
}
//add custom appearance item
exec function capp(path : String)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
template = (CEntityTemplate)LoadResource(path, true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
//remove custom appearance item
exec function rcapp(path : String)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
template = (CEntityTemplate)LoadResource(path, true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
//Hide player model
exec function hideplayer(optional swordstoo : bool)
{
	var inv : CInventoryComponent;
	var witcher : W3PlayerWitcher;
	var ids : array<SItemUniqueId>;
	var size : int;
	var i : int;
	var l_actor : CActor;
	var l_comp : CComponent;
	var template : CEntityTemplate;
	var meshes : array<CComponent>;
	var mesh : CMeshComponent;
	var ii : int;
	
	
	l_actor = thePlayer;
	
	if(!thePlayer.IsCiri())
	{
	witcher = GetWitcherPlayer();
	inv = thePlayer.GetInventory();

	inv.GetAllItems(ids);
	size = ids.Size();
	
	if( size > 0 )
	{
		
		for( i = 0; i < size; i+=1 )
		{
			if(inv.IsItemMounted( ids[i] ) )
				inv.DespawnItem(ids[i]);
		}
		
	}
	
	ids.Clear();
	}
	else
	{
	l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_fur.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_bandaged.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_bandaged_naked.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dirty.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_disguised.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_disguised_02.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_hooded.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_hooded_02.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_wounded.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_03_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_04_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\c_06_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\c_01_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\h_01_wa__ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\h_01_wa__ciri_masked.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\h_01_wa__ciri_wounded.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\h_04_wa__ciri_crying.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\l_01_wa__lingerie_ciri.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_dirty.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_disguised.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_disguised_02.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_fur.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_hooded.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_hooded_02.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\body_01_wa__ciri_dlc_wounded.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	template = (CEntityTemplate)LoadResource("characters\models\main_npc\ciri\h_01_wa__ciri_masked_dlc.w2ent", true);
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
	
	if(swordstoo)
	{
	witcher = GetWitcherPlayer();
	inv = thePlayer.GetInventory();

	inv.GetAllItems(ids);
	size = ids.Size();
	
	if( size > 0 )
	{
		
		for( i = 0; i < size; i+=1 )
		{
			if(inv.IsItemMounted( ids[i] ) )
				inv.DespawnItem(ids[i]);
		}
		
	}
	
	ids.Clear();
	}
	}
	
		meshes = thePlayer.GetComponentsByClassName( 'CMeshComponent' );
		
		if( meshes.Size() > 0 )
		{
			for( ii=0; ii < meshes.Size(); ii+=1 )
			{
				mesh = (CMeshComponent) meshes[ii];
				mesh.SetVisible( false );
			}
		}	
}
//Switch Ciri to Tamara
exec function Tamara(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\hair\c_18_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\h_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\body_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\a2g_01_wa__body.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\i_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\hair\c_18_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\h_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\body_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\a2g_01_wa__body.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\tamara\i_01_wa__tamara.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Switch Ciri to Triss
exec function Triss(enable : bool, head : int, body : int)
{
var l_actor : CActor;
var l_comp : CComponent;
var coiftemplate : CEntityTemplate;
var headtemplate : CEntityTemplate;
var bodytemplate : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

coiftemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\c_01_wa__triss.w2ent", true);

switch( head )
{
case 1:
headtemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\h_01_wa__triss.w2ent", true);
break;
case 2:
headtemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\h_02_wa__triss_tortured.w2ent", true);
break;
case 3:
headtemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\h_03_wa__triss_blood.w2ent", true);
break;
}

switch ( body )
{
case 1:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_01_wa__triss.w2ent", true);
break;
case 2:
coiftemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\c_02_wa__triss.w2ent", true);
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_01_wa__triss_hooded_01.w2ent", true);
break;
case 3:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_01_wa__triss_hooded_02.w2ent", true);
break;
case 4:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_01_wa__triss_tortured.w2ent", true);
break;
case 5:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_02_wa__triss.w2ent", true);
break;
case 6:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\body_03_wa__triss.w2ent", true);
break;
}

if(enable)
{
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(coiftemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(bodytemplate);
}
else
{
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(coiftemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(bodytemplate);

}
}
//Switch Ciri to Yennefer
exec function Yennefer(enable : bool, coif : int, body : int, optional pendant : int)
{
var l_actor : CActor;
var l_comp : CComponent;
var coiftemplate : CEntityTemplate;
var headtemplate : CEntityTemplate;
var bodytemplate : CEntityTemplate;
var legstemplate : CEntityTemplate;
var pendanttemplate : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

headtemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\h_01_wa__yennefer.w2ent", true);
legstemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\l_02_wa__yennefer.w2ent", true);

switch( coif )
{
case 1:
coiftemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\c_01_wa__yennefer.w2ent", true);
break;
case 2:
coiftemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\c_03_wa__yennefer.w2ent", true);
break;
}

switch ( body )
{
case 1:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\b_01_wa__yennefer.w2ent", true);
break;
case 2:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\b_01_wa__yennefer_hooded.w2ent", true);
coiftemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\c_05_wa__yennefer.w2ent", true);
if(enable)
	((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(legstemplate);
break;
case 3:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\b_01_wa__yennefer_hooded_02.w2ent", true);
if(enable)
	((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(legstemplate);
break;
case 4:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\b_03_wa_yennefer.w2ent", true);
if(enable)
	((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(legstemplate);
break;
case 5:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\b_04_wa__yennefer.w2ent", true);
break;
case 6:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\body_yennefer__lingerie.w2ent", true);
break;
case 7:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\twals_01_wa__body_notcensored.w2ent", true);
break;
}
switch( pendant )
{
case 1:
pendanttemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\pendant_01_wa__yennefer.w2ent", true);
break;
case 2:
pendanttemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\pendant_02_wa__yennefer.w2ent", true);
break;
case 3:
pendanttemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\triss\pendant_03_wa__yennefer.w2ent", true);
break;
}

if(enable)
{
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(coiftemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(bodytemplate);
			if(pendant > 0)
				((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(pendanttemplate);
}
else
{
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(coiftemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(bodytemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(legstemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(pendanttemplate);
}
}
//Switch Ciri to Tomira
exec function Tomira(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\hair\c_12_wa__hair_herbalist.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\heads\h_20_wa__quest_fan.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\g_01_wa__body.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\legs\l2_04_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\torso\t2_01_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\shoes\s_05_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager_woman\arms\a_01_wa__skellige_villager.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager_woman\items\i_03_wa__skellige_villager.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\items\i_01_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\items\i_12_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\hair\c_12_wa__hair_herbalist.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\heads\h_20_wa__quest_fan.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\g_01_wa__body.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\legs\l2_04_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\torso\t2_01_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\shoes\s_05_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager_woman\arms\a_01_wa__skellige_villager.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager_woman\items\i_03_wa__skellige_villager.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\items\i_01_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen_woman\items\i_12_wa__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Switch Geralt to Roche
exec function Roche(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\c_01__roche.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\h_01_ma__roche.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\body_01__roche.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\c_01__roche.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\h_01_ma__roche.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\roche\body_01__roche.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Switch Geralt to Letho
exec function Letho(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\letho\h_01_ma__letho.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\letho\body_01__letho.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\letho\h_01_ma__letho.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\main_npc\letho\body_01__letho.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Switch Ciri to Ves
exec function Ves(enable : bool, head : int)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;
var headtemplate : CEntityTemplate;
var hairtemplate : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
hairtemplate = (CEntityTemplate)LoadResource("characters\models\common\woman_average\hair\c_18_wa__ves.w2ent", true);

switch ( head )
{
case 1:
headtemplate = (CEntityTemplate)LoadResource("characters\models\secondary_npc\ves\h_01_wa__ves.w2ent", true);
break;
case 2:
headtemplate = (CEntityTemplate)LoadResource("characters\models\secondary_npc\ves\h_01_wa__ves_q403.w2ent", true);
break;
}

if(enable)
{
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(hairtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(headtemplate);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\ves\body_01_wa__ves.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(hairtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(headtemplate);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\ves\body_01_wa__ves.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Switch Geralt to Lambert
exec function Lambert(enable : bool, body : int, optional fabulous : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;
var bodytemplate : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

switch ( body )
{
case 1:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\body_01_ma__lambert.w2ent", true);
break;
case 2:
bodytemplate = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\body_01_ma__lambert_blood.w2ent", true);
break;
}
if(fabulous)
{
bodytemplate = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\ta0_01_ma__body_lambert.w2ent", true);
if(enable)
{
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\c_01_ma__lambert.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\d_01_ma__lambert.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\t3_01_ma__lambert.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\common\man_average\body\g_01_ma__body", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
}

if(enable)
{

			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\h_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(bodytemplate);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\i_04_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\s_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\h_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\i_04_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\s_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\ta0_01_ma__body_lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\c_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\d_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\lambert\t3_01_ma__lambert.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\man_average\body\g_01_ma__body", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(bodytemplate);
}
}
//Switch Geralt to Eskel
exec function Eskel(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\h_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\body_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\s_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
//template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\i_04_ma__eskel.w2ent", true);
//((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);		
}
else
{
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\h_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\body_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\s_01_ma__eskel.w2ent", true);
((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
//template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\eskel\i_04_ma__eskel.w2ent", true);
//((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);	
}
}
//Switch Geralt to mirror man
exec function Mephisto(enable : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\mirror_man\h_01_ma__mirror_man.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\man_average\body\n_01_ma__body.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\torso\t2_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\legs\l0_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\shoes\s_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\arms\a_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\gloves\g_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\items\i_02_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\items\i_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\medic\items\i_11_ma__medic.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\wizard\items\i_15_ma__wizard.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\medic\items\i_12_ma__medic.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\burrows_soltis\i_01_ma__burrows_soltis.w2ent", true);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);
}
else
{
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\mirror_man\h_01_ma__mirror_man.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\common\man_average\body\n_01_ma__body.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\torso\t2_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\legs\l0_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\shoes\s_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\arms\a_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\gloves\g_01_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\items\i_02_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\novigrad_citizen\items\i_03_ma__novigrad_citizen.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\medic\items\i_11_ma__medic.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\wizard\items\i_15_ma__wizard.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\medic\items\i_12_ma__medic.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
			template = (CEntityTemplate)LoadResource("characters\models\secondary_npc\burrows_soltis\i_01_ma__burrows_soltis.w2ent", true);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Give Geralt a cape
exec function cape(enable : bool, optional hideweapons : bool, optional grey : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;
var inv : CInventoryComponent;
var ids : array<SItemUniqueId>;
var size : int;
var i : int;

l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );

if(enable)
{
	if(hideweapons)
	{
	inv = thePlayer.GetInventory();

	inv.GetAllItems(ids);
	size = ids.Size();
	
	if( size > 0 )
	{
		
		for( i = 0; i < size; i+=1 )
		{
			if(inv.IsItemWeapon(ids[i]) || inv.IsItemCrossbow(ids[i]))
				inv.DespawnItem(ids[i]);
				
			if(inv.GetItemCategory(ids[i]) == 'steel_scabbards')
				inv.DespawnItem(ids[i]);
				
			if(inv.GetItemCategory(ids[i]) == 'silver_scabbards')
				inv.DespawnItem(ids[i]);
		}
		
	}
	
	ids.Clear();
	}
	template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager\items\i_06_ma__skellige_villager_px.w2ent", true);
	
	if(grey)
		template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager\items\i_06_mb__skellige_villager_px.w2ent", true);
		
	((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(template);;
}
else
{
	template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager\items\i_06_ma__skellige_villager_px.w2ent", true);
	
		if(grey)
			template = (CEntityTemplate)LoadResource("characters\models\crowd_npc\skellige_villager\items\i_06_mb__skellige_villager_px.w2ent", true);
		
	((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(template);
}
}
//Spawn NPC from path
exec function spawnfrompath(path : string, optional quantity : int, optional distance : float, optional isHostile : bool )
{
	var ent : CEntity;
	var pos, cameraDir, player, posFin, normal, posTemp : Vector;
	var rot : EulerAngles;
	var i, sign : int;
	var s,r,x,y : float;
	var template : CEntityTemplate;
	var resourcePath	: string;
	quantity = Max(quantity, 1);
	
	rot = thePlayer.GetWorldRotation();	
	rot.Yaw += 180;		
	
	
	cameraDir = theCamera.GetCameraDirection();
	
	if( distance == 0 ) distance = 3; 
	cameraDir.X *= distance;	
	cameraDir.Y *= distance;
	
	
	player = thePlayer.GetWorldPosition();
	
	
	pos = cameraDir + player;	
	pos.Z = player.Z;
	
	
	posFin.Z = pos.Z;			
	s = quantity / 0.2;			
	r = SqrtF(s/Pi());
	
	
	template = (CEntityTemplate)LoadResource(path,true);

	for(i=0; i<quantity; i+=1)
	{		
		x = RandF() * r;			
		y = RandF() * (r - x);		
		
		if(RandRange(2))					
			sign = 1;
		else
			sign = -1;
			
		posFin.X = pos.X + sign * x;	
		
		if(RandRange(2))					
			sign = 1;
		else
			sign = -1;
			
		posFin.Y = pos.Y + sign * y;	
		
			if(theGame.GetWorld().StaticTrace( posFin + Vector(0,0,5), posFin - Vector(0,0,5), posTemp, normal ))
			{
				posFin = posTemp;
			}
			
			ent = theGame.CreateEntity(template, posFin, rot);
			
		if( isHostile )
		{
			((CActor)ent).SetTemporaryAttitudeGroup( 'hostile_to_player', AGP_Default );
		}
	}
}
//Switch Ciri to Shani
exec function Shani(enable : bool, head : int, body : int, optional item : bool, optional rowan : bool, optional naked : bool, optional lingerie : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;
var headtemplate : CEntityTemplate;
var hairtemplate : CEntityTemplate;
var bodytemplate : CEntityTemplate;
var itemtemplate : CEntityTemplate;
var rowantemplate : CEntityTemplate;
var lowerlingerietemplate : CEntityTemplate;
var upperlingerietemplate : CEntityTemplate;
var lowerbodytemplate	: CEntityTemplate;
var upperbodytemplate : CEntityTemplate;


l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
itemtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\i_01_wa__shani.w2ent", true);
hairtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\c_01_wa__shani_hair.w2ent", true);
rowantemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\c_01_wa__shani.w2ent", true);
lowerlingerietemplate = (CEntityTemplate)LoadResource("characters\models\crowd_npc\lingerie\legs\l_02_wa__lingerie.w2ent", true);
upperlingerietemplate = (CEntityTemplate)LoadResource("characters\models\crowd_npc\lingerie\torso\t_02_wa__lingerie.w2ent", true);
lowerbodytemplate = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\ls_01_wa__body.w2ent", true);
upperbodytemplate = (CEntityTemplate)LoadResource("characters\models\common\woman_average\body\a2g_01_wa__body.w2ent", true);

switch ( head )
{
case 1:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\h_01_wa__shani.w2ent", true);
break;
case 2:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\h_01_wa__shani_dirty.w2ent", true);
break;
}

switch ( body )
{
case 1:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\b_01_wa__shani.w2ent", true);
break;
case 2:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\b_01_wa__shani_dirty.w2ent", true);
break;
case 3:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\b_02_wa__shani.w2ent", true);
break;
case 4:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\b_03_wa__shani.w2ent", true);
break;
case 5:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\secondary_npc\shani\b_04_wa__shani.w2ent", true);
break;
}

if(naked == true)
{
bodytemplate = (CEntityTemplate)LoadResource("characters\models\main_npc\yennefer\twals_01_wa__body_notcensored.w2ent", true);
}

if(naked == true && lingerie == true && enable)
{
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(upperlingerietemplate);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(lowerlingerietemplate);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(upperbodytemplate);
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(lowerbodytemplate);
bodytemplate = (CEntityTemplate)LoadResource("", true);
}

if(enable)
{
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(hairtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(bodytemplate);
}
else
{
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(hairtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(bodytemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(itemtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(rowantemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(upperlingerietemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(lowerlingerietemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(lowerbodytemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(upperbodytemplate);		
}
if(item == true)
{
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(itemtemplate);
}

if(rowan == true)
{
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(rowantemplate);
}
}
//roll with class to impress every lass
exec function proudwalk(val : float)
{
thePlayer.SetBehaviorVariable( 'proudWalk', val );
}
//Quick control for other kind of custom anims
exec function forceanim(nam : name)
{
thePlayer.PlayerStartAction( 1,nam );
}
//I need to move it, move it
exec function salsa(type : int)
{
var salsatype : CName;

switch( type )
{
case 1:
salsatype = 'locomotion_salsa_cycle_02';
break;
case 2:
salsatype = 'locomotion_salsa_cycle_01';
break;
case 3:
salsatype = 'locomotion_salsa_short_cycle_01';
break;
}
thePlayer.PlayerStartAction( 1,salsatype );
}
//Force all surrounding NPCs to play animation
exec function dothisformeall(aname : name)
{
	var i :int;
	var l_actor 		: CActor;
	var l_actors		: array<CActor>;
	var l_aiTree		: CAIPlayAnimationSlotAction;
	
	l_actors = GetActorsInRange( thePlayer, 1000, 99 );
	
	l_aiTree = new CAIPlayAnimationSlotAction in l_actor;
	l_aiTree.OnCreated();
	
	l_aiTree.slotName = 'NPC_ANIM_SLOT';
	l_aiTree.animName = aname;
	
	for	( i = 0; i < l_actors.Size(); i+= 1 )
	{
		l_actor = (CActor) l_actors[i];
		if ( l_actor == thePlayer )
			continue;
		l_actor.ForceAIBehavior( l_aiTree, BTAP_Emergency);
	}
}
//Force NPC to play animation
exec function dothisforme(aname : name)
{
	var act : CGameplayEntity;
	var l_aiTree		: CAIPlayAnimationSlotAction;
	var l_actor : CActor;
	
	act = thePlayer.GetDisplayTarget();
	l_actor = ((CActor)act);
	
	l_aiTree = new CAIPlayAnimationSlotAction in l_actor;
	l_aiTree.OnCreated();
	
	l_aiTree.slotName = 'NPC_ANIM_SLOT';
	l_aiTree.animName = aname;
	
	l_actor.ForceAIBehavior( l_aiTree, BTAP_Emergency);
}
//Blur output frame
exec function blurscreen(intensity : float)
{
FullscreenBlurSetup(intensity);
}
//Some kind of silly radial blur, more like actual ingame FX rather than post process
exec function radialblur(position : bool,amount : float, waveamount : float, wavespeed : float, wavefreq : float)
{
var effectpos : Vector;

if(position == true)
{
effectpos = thePlayer.GetWorldPosition();
}
else
{
effectpos = theCamera.GetCameraPosition();
}
RadialBlurSetup( effectpos, amount, waveamount, wavespeed, wavefreq );
}
//Turn sepia on
exec function sepiaon()
{
theGame.StartSepiaEffect(1.0);
}
//And turn it off
exec function sepiaoff()
{
theGame.StopSepiaEffect(1.0);
}
//Force HoS painting environment/postprocess
exec function painting()
{
var environment : CEnvironmentDefinition;
//				environmentDefinitions.PushBack("dlc\ep1\data\environment\definitions\in_progress\q604\q604_unmasking.env");	
//				environmentDefinitions.PushBack("dlc\ep1\data\environment\definitions\in_progress\q604\q604_unmasking_dark.env");	
//				environmentDefinitions.PushBack("dlc\ep1\data\environment\definitions\in_progress\q604\q604_unmasking_shadow_more_details.env");	

environment = ( CEnvironmentDefinition )LoadResource( "dlc\ep1\data\environment\definitions\in_progress\q604\q604_unmasking.env", true );
theGame.SetEnvironmentID(ActivateEnvironmentDefinition(environment,100,1,1));
}
//Rotate player because it's nearly impossible to rotate precisely with normal controls
exec function rotme(deg : float)
{
var currentrotation : EulerAngles; 
var testcomp : CComponent;

testcomp = thePlayer.GetComponentByClassName('CAnimatedComponent');
currentrotation = thePlayer.GetWorldRotation();
currentrotation.Yaw = deg;
testcomp.SetRotation(currentrotation);
}
//shift model upward/downward
exec function shiftz(val : float)
{
var testcomp : CComponent;

testcomp = thePlayer.GetComponentByClassName('CAnimatedComponent');
testcomp.SetPosition(Vector(0,0,0,1));
testcomp.SetPosition(Vector(0,0,val,1));
}
//Go to "through time and space" 4 in 1 level
exec function gotoSpiral()
{
	theGame.ScheduleWorldChangeToMapPin( "levels\the_spiral\spiral.w2w", '' );
	theGame.RequestAutoSave( "fast travel", true );
}
//Move to elven city within the spiral
exec function ecity()
{
if(theGame.GetCommonMapManager().GetCurrentArea() == 7)
	thePlayer.Teleport(Vector(1452.47,-1147.422,117.76,1));
}
//Move to frozen town within the spiral
exec function frozentown()
{
if(theGame.GetCommonMapManager().GetCurrentArea() == 7)
	thePlayer.Teleport(Vector(1088.04,-3615.37,419,1));
}
//Move to gas forest within the spiral
exec function gasforest()
{
if(theGame.GetCommonMapManager().GetCurrentArea() == 7)
	thePlayer.Teleport(Vector(-681.51,-2315.17,84,1));
}
//Move to desert land within the spiral
exec function deadland()
{
if(theGame.GetCommonMapManager().GetCurrentArea() == 7)
	thePlayer.Teleport(Vector(-1456.8,-2543.74,173.23,1));
}
//Go to the island of mist
exec function gotoIOMist()
{
	theGame.ScheduleWorldChangeToMapPin( "levels\island_of_mist\island_of_mist.w2w", '' );
	theGame.RequestAutoSave( "fast travel", true );
}
//Teleport to shore on the island of mist
exec function iomshore()
{
if(theGame.GetCommonMapManager().GetCurrentArea() == 6)
	thePlayer.Teleport(Vector(-7.73,294.48,0.41,1));
}
//Force cutscene environment preset
exec function cutscenelighting()
{
var environment : CEnvironmentDefinition;

environment = ( CEnvironmentDefinition )LoadResource( "environment\definitions\cutscenes_definition\cutscen_definition_global.env", true );
theGame.SetEnvironmentID(ActivateEnvironmentDefinition(environment,100,1,1));
}
//Load any environment
exec function customenvironment(path : String)
{
var environment : CEnvironmentDefinition;

environment = ( CEnvironmentDefinition )LoadResource( path, true );
theGame.SetEnvironmentID(ActivateEnvironmentDefinition(environment,100,1,1));
}
//Switch Geralt to Olgierd
exec function Olgierd(enable : bool, head : int, body : int, optional item : bool)
{
var l_actor : CActor;
var l_comp : CComponent;
var template : CEntityTemplate;
var headtemplate : CEntityTemplate;
var bodytemplate : CEntityTemplate;
var itemtemplate : CEntityTemplate;


l_actor = thePlayer;
l_comp = l_actor.GetComponentByClassName( 'CAppearanceComponent' );
itemtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\i_01_ma__olgierd.w2ent", true);

switch ( head )
{
case 1:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\h_01_ma__olgierd.w2ent", true);
break;
case 2:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\h_01_ma__olgierd_cutoff.w2ent", true);
break;
case 3:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\h_01_ma__olgierd_cutoff_healed.w2ent", true);
break;
case 4:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\h_01_ma__olgierd_old_morph.w2ent", true);
break;
case 5:
headtemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\h_01_ma__olgierd_young.w2ent", true);
break;
}

switch ( body )
{
case 1:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\olgierd.w2ent", true);
break;
case 2:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\olgierd_blood.w2ent", true);
break;
case 3:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\olgierd_blood_hand.w2ent", true);
break;
case 4:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\olgierd_morph.w2ent", true);
break;
case 5:
bodytemplate = (CEntityTemplate)LoadResource("dlc\ep1\data\characters\models\main_npc\olgierd\olgierd_young.w2ent", true);
break;
}

if(enable)
{
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(bodytemplate);
}
else
{
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(headtemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(bodytemplate);
			((CAppearanceComponent)l_comp).ExcludeAppearanceTemplate(itemtemplate);		
}
if(item == true)
{
((CAppearanceComponent)l_comp).IncludeAppearanceTemplate(itemtemplate);
}
}
//Play a mimic animation
exec function playmimic(anim : name)
{
((CActor)thePlayer).PlayMimicAnimationAsync(anim);
}
//Set player appearance
exec function myapp(nam : name)
{
((CActor)thePlayer).SetAppearance( nam );
}
//Morph mesh controls
exec function morphme(ratio : float, time : float)
{
var l_actor : CActor;
var l_comp : array< CComponent >;
var i, size : int;

l_actor = thePlayer;
l_comp = l_actor.GetComponentsByClassName( 'CMorphedMeshManagerComponent' );
size = l_comp.Size();

for ( i=0; i<size; i+= 1 )
	{
		((CMorphedMeshManagerComponent)l_comp[ i ]).SetMorphBlend( ratio, time );
	}
}
//Play cooked effect
exec function playerplayeffect(enam : CName)
{
thePlayer.PlayEffect(enam);
}
//Stop all effects
exec function playerstopeffects()
{
thePlayer.StopAllEffects(); 
}