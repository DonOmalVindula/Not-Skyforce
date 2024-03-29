//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function MoveToToy::create( %this )
{
    // Initialize the toys settings.
    echo("Game Start");
    exec("./scripts/faceObjectBehavior.cs");
    exec("./scripts/moveTowardBehavior.cs");
    exec("./scripts/spawnAreaBehavior.cs");
    MoveToToy.moveSpeed = 110;
    MoveToToy.count = 0;
	MoveToToy.livesLeft = 3;
    MoveToToy.trackMouse = true;
    MoveToToy.Music = "MoveToToy:titleMusic";
    MoveToToy.FireMusic = "MoveToToy:gunFire";
    MoveToToy.Score = 0;
    // Add the custom controls.
    addNumericOption("Move Speed", 1, 150, 1, "setMoveSpeed", MoveToToy.moveSpeed, true, "Sets the linear speed to use when moving to the target position.");
    addFlagOption("Track Mouse", "setTrackMouse", MoveToToy.trackMouse, false, "Whether to track the position of the mouse or not." );

    %this.createSpawnAreaBehavior();
    %this.createMoveTowardBehavior();
    // Reset the toy initially.
    MoveToToy.reset();        
}

//-----------------------------------------------------------------------------

function MoveToToy::destroy( %this )
{
    alxStopAll();
}

function MoveToToy::createEndBox( %this )
{
	// Create the sprite.
	%object = new Sprite()
	{
    	class = "EndBox";
	};
	%object.BodyType = static;
	%object.Position = "0 -40";
	%object.Layer = 30;
	%object.Size = 100 SPC 5;
	%object.createPolygonBoxCollisionShape();
	%object.Image = "MoveToToy:checkered";
	%object.Frame = getRandom(0,55);
	SandboxScene.add( %object );
}

function MoveToToy::createHeart1( %this )
{
	%object = new Sprite()
	{
    	class = "heart1";
	};
	%object.BodyType = static;
	%object.Position = "-24 31";
	%object.Layer = 30;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function MoveToToy::createHeart2( %this )
{
	%object = new Sprite()
	{
    	class = "heart2";
	};
	%object.BodyType = static;
	%object.Position = "-18 31";
	%object.Layer = 30;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function MoveToToy::createHeart3( %this )
{
	%object = new Sprite()
	{
    	class = "heart3";
	};
	%object.BodyType = static;
	%object.Position = "-12 31";
	%object.Layer = 30;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function MoveToToy::createHeart10( %this )
{
	%object = new Sprite()
	{
    	class = "heart3";
	};
	%object.BodyType = static;
	%object.Position = "-24 31";
	%object.Layer = 30;
	%object.BlendColor = Black;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function MoveToToy::createHeart20( %this )
{
	%object = new Sprite()
	{
    	class = "heart3";
	};
	%object.BodyType = static;
	%object.Position = "-18 31";
	%object.Layer = 30;
	%object.BlendColor = Black;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function MoveToToy::createHeart30( %this )
{
	%object = new Sprite()
	{
    	class = "heart3";
	};
	%object.BodyType = static;
	%object.Position = "-12 31";
	%object.Layer = 30;
	%object.BlendColor = Black;
	%object.Size = 5 SPC 3.75 ;	
	%object.Image = "MoveToToy:heart";	
	SandboxScene.add( %object );
}

function EndBox::onCollision( %this, %object, %collisionDetails )
{
	%this.count++; 	
	if (%this.count >= 5)
	{
    	switch(MoveToToy.livesLeft)
    	{
        	case 3:
            	MoveToToy.createHeart30();
        	case 2:
            	MoveToToy.createHeart20();
        	case 1:
            	MoveToToy.createHeart10();
        	case 0:
            	exitGame(); //Make this
        	default:
            	nope();
    	}           	
    	MoveToToy.livesLeft--;
    	%this.count = 0;
	}
}


//-----------------------------------------------------------------------------

function MoveToToy::reset( %this )
{
    // Clear the scene.
    SandboxScene.clear();

    %this.Score = 0;
    %this.createEndBox();
	%this.createHeart1();  
	%this.createHeart2();
	%this.createHeart3();

	//Add health text
	new TextSprite()
	{
    	Scene = SandboxScene;
    	Font = "ToyAssets:ArialFont";
    	FontSize = 3;
    	Text = "LIVES LEFT - ";
    	Position = "0 25";
    	Size = "90 15";
    	OverflowModeY = "visible";
    	BlendColor = "255 0 0 1";
	};

    
    // Create background.
    %this.createBackground();
    %this.createFarScroller();
    alxPlay(MoveToToy.Music);

    // Create target.
    %this.createTarget();
    
    // Create sight.
    %this.createSight();

    %this.startTimer( "createBullet", 200 );
    %this.createEnemy();

    %this.setupSpawnPoints();
    //%this.startTimer( "createEnemy", 500 );
}

//-----------------------------------------------------------------------------

function MoveToToy::createBackground( %this )
{    
    // Create the sprite.
    %object = new Sprite();
    
    // Set the sprite as "static" so it is not affected by gravity.
    %object.setBodyType( static );
       
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 0";

    // Set the size.        
    %object.Size = "100 75";
    
    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set an image.
    %object.Image = "MoveToToy:skyBackground";
    
    // Set the blend color.
    //%object.BlendColor = SlateGray;
            
    // Add the sprite to the scene.
    SandboxScene.add( %object );    
}

function MoveToToy::createFarScroller( %this )
{    
    // Create the scroller.
    %object = new Scroller();
    
    // Note this scroller for the touch controls.
    MoveToToy.FarScroller = %object;
    
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 -10";

    // Set the size.        
    %object.Size = "100 100";

    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set the scroller to use a static image.
    %object.Image = "MoveToToy:checkered";
    
    // We don't really need to do this as the frame is set to zero by default.
    %object.Frame = 0;

    // Set the scroller moving in the X axis.
    %object.ScrollY = 25;
    
    // Set the scroller to only show half of the static image in the X axis.
    %object.RepeatX = 1.0;
        
    // Add the sprite to the scene.
    SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function MoveToToy::createSight( %this )
{
    // Create the sprite.
    %object = new Sprite();
    
    // Set the sight object.
    MoveToToy.SightObject = %object;
    
    // Set the static image.
    %object.Image = "MoveToToy:spaceship";

    // Set the blend color.
    //%object.BlendColor = Lime;
    
    // Set the transparency.
    %object.setBlendAlpha( 1.0 );
    %object.setDefaultDensity( 10000 );
    
    // Set a useful size.
    %object.Size = 11;
    
    // Set the sprite rotating to make it more interesting.
    %object.AngularVelocity = 0;
    //%object.createCircleCollisionShape( 11 * 0.48 );
    
    // Add to the scene.
    SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function MoveToToy::createTarget( %this )
{
    // Create the sprite.
    %object = new Sprite();

    // Set the target object.
    MoveToToy.TargetObject = %object;
    
    // Set the static image.
    %object.Image = "MoveToToy:spaceship";
    
    // Set the blend color.
    %object.BlendColor = DarkOrange;
    
    // Set a useful size.
    %object.Size = 10;
        
    // Set the sprite rotating to make it more interesting.
    %object.AngularVelocity = 0;
    
    // Add to the scene.
    //SandboxScene.add( %object );    
}

//-----------------------------------------------------------------------------

function MoveToToy::setMoveSpeed( %this, %value )
{
    %this.moveSpeed = %value;
}

//-----------------------------------------------------------------------------

function MoveToToy::setTrackMouse( %this, %value )
{
    %this.trackMouse = %value;
}

//-----------------------------------------------------------------------------

// function MoveToToy::onTouchDown(%this, %touchID, %worldPosition)
// {
//     // Set the target to the touched position.
//     MoveToToy.TargetObject.Position = %worldPosition;
    
//     // Move the sight to the touched position.
//     MoveToToy.SightObject.MoveTo( %worldPosition, MoveToToy.moveSpeed );
// }

//-----------------------------------------------------------------------------

function MoveToToy::onTouchDragged(%this, %touchID, %worldPosition)
{
    // Finish if not tracking the mouse.
    if ( !MoveToToy.trackMouse )
        return;
        
    // Set the target to the touched position.
    MoveToToy.TargetObject.Position = %worldPosition;
    
    // Move the sight to the touched position.
    MoveToToy.SightObject.MoveTo( %worldPosition, MoveToToy.moveSpeed );     
}

//-----------------------------------------------------------------------------

function MoveToToy::createBullet( %this, %position )
{
    // Create an Bullet.
    %object = new Sprite();
    //alxPlay(MoveToToy.titleMusic);
    %object.Position = MoveToToy.SightObject.Position;
    %object.Size = 4;
    %object.Image = "MoveToToy:Bullet";
    %object.ImageFrame = getRandom(0,3);
    %object.SceneLayer = 10;
    %object.setDefaultDensity( 0 );
    %object.createCircleCollisionShape( 4 * 0.4 );
    %object.setLinearVelocity( 0, 40 );
    %object.setLifetime( 1.7 );  
    SandboxScene.add( %object );
}

//-----------------------------------------------------------------------------

function MoveToToy::createEnemy ( %this )
{
    %position = "0 50";

    %object = new Sprite()
    {
        class = "Enemy";
    };
    %object.Position = %position;
    %object.Size = 5;
    %object.Image = "MoveToToy:Enemy";
    %object.AngularVelocity = 0 ;
    %object.setLinearVelocity( 0, -10 ); 
    %object.setDefaultDensity( 1000 );
    %object.createCircleCollisionShape( 5 * 0.48 );
    %object.CollisionCallback = true;
    %this.objectTemplate = %object;

    SandboxScene.add( %objectTemplate );   
    %this.objectTemplate.setEnabled(0);
    
}

function MoveToToy::setupSpawnPoints(%this)
{
    %amount = 5;
    // %spawners = 340;
    // %n=0;

    // for( %n = 40; %n <= 340; %n+=40; )
    // {        
    //     %this.createSpawnPoint("-30 %n", %amount);
    //     %this.createSpawnPoint("0 %n", %amount);
    //     %this.createSpawnPoint("30 %n", %amount);
    // }

    %this.createSpawnPoint("-30 40", %amount);
    %this.createSpawnPoint("0 40", %amount);
    %this.createSpawnPoint("30 40", %amount);

    %this.createSpawnPoint("-25 80", %amount);
    %this.createSpawnPoint("10 80", %amount);
    %this.createSpawnPoint("30 80", %amount);
 
    %this.createSpawnPoint("-32 120", %amount);
    %this.createSpawnPoint("0 120", %amount);
    %this.createSpawnPoint("24 120", %amount);
 
    %this.createSpawnPoint("-25 160", %amount);
    %this.createSpawnPoint("0 160", %amount);
    %this.createSpawnPoint("18 160", %amount);

    %this.createSpawnPoint("-30 200", %amount);
    %this.createSpawnPoint("0 200", %amount);
    %this.createSpawnPoint("30 200", %amount);

    %this.createSpawnPoint("-25 240", %amount);
    %this.createSpawnPoint("10 240", %amount);
    %this.createSpawnPoint("30 240", %amount);
 
    %this.createSpawnPoint("-32 280", %amount);
    %this.createSpawnPoint("0 280", %amount);
    %this.createSpawnPoint("24 280", %amount);
 
    %this.createSpawnPoint("-25 320", %amount);
    %this.createSpawnPoint("0 320", %amount);
    %this.createSpawnPoint("18 320", %amount);

}

function MoveToToy::createSpawnPoint(%this, %position, %amount)
{
    %spawnPoint = new sceneObject()
    {
        size = "10 10";
        position = %position;
    };

    %spawnPointBehavior = %this.spawnAreaBehavior.createInstance();
    %spawnPointBehavior.initialize(%this.objectTemplate, %amount, 0.5, 0, true, "Area");
    %spawnPoint.addBehavior(%spawnPointBehavior);

    SandboxScene.add(%spawnPoint);

}

function MoveToToy::createSpawnAreaBehavior(%this)
{
    // Create the named template and retain it as a custom field on this toy
    %this.spawnAreaBehavior = new BehaviorTemplate(SpawnAreaBehavior);

    // Fill in the details of the behavior
    %this.spawnAreaBehavior.friendlyName = "Spawn Area";
    %this.spawnAreaBehavior.behaviorType = "AI";
    %this.spawnAreaBehavior.description  = "Spawns objects inside the area of this object";

    // Add the custom behavior fields
    %this.spawnAreaBehavior.addBehaviorField(object, "The object to clone", object, "", sceneObject);
    %this.spawnAreaBehavior.addBehaviorField(count, "The number of objects to clone (-1 for infinite)", int, 50);
    %this.spawnAreaBehavior.addBehaviorField(spawnTime, "The time between spawns (seconds)", float, 2.0);
    %this.spawnAreaBehavior.addBehaviorField(spawnVariance, "The variance in the spawn time (seconds)", float, 1.0);
    %this.spawnAreaBehavior.addBehaviorField(autoSpawn, "Automatically start/stop spawning", bool, true);

    %spawnLocations = "Area" TAB "Edges" TAB "Center" TAB "Top" TAB "Bottom" TAB "Left" TAB "Right";
    %this.spawnAreaBehavior.addBehaviorField(spawnLocation, "The are in which objects can be spawned", enum, "Area", %spawnLocations);

    // Add the BehaviorTemplate to the scope set so it is destroyed when the module is unloaded
    MoveToToy.add(%this.spawnAreaBehavior);
}

function MoveToToy::setSpawnAmount(%this, %value)
{
    %this.spawnAmount = %value;
}

function MoveToToy::createMoveTowardBehavior(%this)
{
    // Create the named template and retain it as a custom field on this toy
    %this.moveTowardBehavior = new BehaviorTemplate(MoveTowardBehavior);

    // Fill in the details of the behavior
    %this.moveTowardBehavior.friendlyName = "Move Toward";
    %this.moveTowardBehavior.behaviorType = "AI";
    %this.moveTowardBehavior.description  = "Set the object to move toward another object";

    // Add the custom behavior fields
    %this.moveTowardBehavior.addBehaviorField(target, "The object to move toward", object, "", sceneObject);
    %this.moveTowardBehavior.addBehaviorField(speed, "The speed to move toward the object at (world units per second)", float, 2.0);

    // Add the BehaviorTemplate to the scope set so it is destroyed when the module is unloaded
    MoveToToy.add(%this.moveTowardBehavior);
}

//----------------------------------------------------------

function Enemy::onCollision( %this, %object, %collisionDetails )
{
    %positionDelta = Vector2Sub( %object.Position, %this.Position );
    %angle = -mRadToDeg( mAtan( %positionDelta._0, %positionDelta._1 ) );

    MoveToToy.Score++;
    echo(MoveToToy.Score);
    
    // Fetch contact position.
    %contactPosition = %collisionDetails._4 SPC %collisionDetails._5;
    
    // Calculate total impact force.
    %impactForce = mAbs(%collisionDetails._6 / 100) + mAbs(%collisionDetails._7 / 20);
    
    // Create explosion.
    %player = new ParticlePlayer();
    %player.BodyType = static;
    %player.Particle = "ToyAssets:impactExplosion";
    %player.Position = %contactPosition;
    %player.Angle = %angle;
    %player.SizeScale = mClamp( %impactForce, 0.1, 10 );
    %player.SceneLayer = 0;
    SandboxScene.add( %player ); 
    alxPlay(MoveToToy.FireMusic); 

    // Delete the bullet.
    //echo("Hello");
    %object.safeDelete();
    %this.safeDelete();
}