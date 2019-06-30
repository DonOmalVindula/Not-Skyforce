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
    MoveToToy.moveSpeed = 110;
    MoveToToy.trackMouse = true;
    MoveToToy.Music = "MoveToToy:titleMusic";
    // Add the custom controls.
    addNumericOption("Move Speed", 1, 150, 1, "setMoveSpeed", MoveToToy.moveSpeed, true, "Sets the linear speed to use when moving to the target position.");
    addFlagOption("Track Mouse", "setTrackMouse", MoveToToy.trackMouse, false, "Whether to track the position of the mouse or not." );

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
    alxPlay(MoveToToy.Music);

    // Create target.
    %this.createTarget();
    
    // Create sight.
    %this.createSight();

    %this.startTimer( "createAsteroid", 200 );
    %this.startTimer( "createEnemy", 3000 );
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
    
    // Set a useful size.
    %object.Size = 11;
    
    // Set the sprite rotating to make it more interesting.
    %object.AngularVelocity = 0;
    
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

function MoveToToy::createAsteroid( %this, %position )
{
    // Create an asteroid.
    %object = new Sprite();
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
    %object.setDefaultDensity( 10000 );
    %object.createCircleCollisionShape( 5 * 0.48 );
    %object.CollisionCallback = true;
    SandboxScene.add( %object );    
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
    %object.Trail.LinearVelocity = 0;
    %object.Trail.AngularVelocity = 0;
    %object.Trail.safeDelete();
    %object.safeDelete();  
}