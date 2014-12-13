2DPlatformer
============

2d Platformer game



http://xnacoding.blogspot.se/2013/12/entity-component-system.html
 
world.RegisterRenderSystem(new TiledMapRenderSystem());

world.RegisterSystem(new BasicControllingSystem());
world.RegisterSystem(new TiledMapCollisionSystem());
world.RegisterSystem(new MovementSystem());
world.RegisterSystem(new ColliderUpdateSystem());
world.RegisterRenderSystem(new CollisionBoxRenderSystem());
world.RegisterSystem(new Camera2DSystem()); 
world.RegisterRenderSystem(new Render2DSystem());
 
 
Entity mapEntity = world.CreateEntity();
mapEntity.AddComponent(new TiledMapComponent("Content/rpgtest.tmx"));
 
Entity player = world.CreateEntity();
player.AddComponent(new PositionComponent(0, 0));
player.AddComponent(new MovementComponent());
player.AddComponent(new SpriteComponent("Blue-Monster"));
player.AddComponent(new BasicControllerComponent());
player.AddComponent(new Camera2D(true, "", new Vector2(320, 320), new Rectangle(0, 0, 1600, 1600)));
player.AddComponent(new ColliderComponent());
