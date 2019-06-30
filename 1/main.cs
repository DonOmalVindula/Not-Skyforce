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
    MoveToToy.trackMouse = true;
    MoveToToy.Music = "MoveToToy:titleMusic";
    MoveToToy.FireMusic = "MoveToToy:gunFire";
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

//-----------------------------------------------------------------------------

function MoveToToy::reset( %this )
{
    // Clear the scene.
    SandboxScene.clear();
    
    // Create background.
    %this.createBackground();
    %this.createFarScroller();
    //alxPlay(MoveToToy.Music);

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
    %object.setDefaultDensity( 0.2 );
    %object.createCircleCollisionShape( 4 * 0.4 );
    %object.setLinearVelocity( 0, 40 );
    %object.setLifetime( 2 );  
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
    %object.Image = "MoveToToy:Tires";
    %object.AngularVelocity = -5;
    %object.setLinearVelocity( 0, -10 ); 
    %object.setDefaultDensity( 10 );
    %object.createCircleCollisionShape( 5 * 0.48 );
    %object.CollisionCallback = true;
    %this.objectTemplate = %object;

    SandboxScene.add( %objectTemplate );   
    %this.objectTemplate.setEnabled(0);
    
}

function MoveToToy::setupSpawnPoints(%this)
{
    %amount = 600 / 6;

    // Creating four in the corners of the space
    %this.createSpawnPoint("-30 40", %amount);
    %this.createSpawnPoint("0 40", %amount);
    %this.createSpawnPoint("30 40", %amount);
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
    // %positionDelta = Vector2Sub( %object.Position, %this.Position );
    // %angle = -mRadToDeg( mAtan( %positionDelta._0, %positionDelta._1 ) );
    
    // // Fetch contact position.
    // %contactPosition = %collisionDetails._4 SPC %collisionDetails._5;
    
    // // Calculate total impact force.
    // %impactForce = mAbs(%collisionDetails._6 / 100) + mAbs(%collisionDetails._7 / 20);
    
    // // Create explosion.
    // %player = new ParticlePlayer();
    // %player.BodyType = static;
    // %player.Particle = "MoveToToy:impactExplosion";
    // %player.Position = %contactPosition;
    // %player.Angle = %angle;
    // %player.SizeScale = mClamp( %impactForce, 0.1, 10 );
    // %player.SceneLayer = 0;
    // SandboxScene.add( %player );

    // Delete the bullet.
    //echo("Hello");
    %object.safeDelete();
    // %object.Trail.LinearVelocity = 0;
    // %object.AngularVelocity = 0;
    // %object.Trail.safeDelete();
    // %object.safeDelete();  
}