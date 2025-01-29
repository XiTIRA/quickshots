using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ecs;

public class EcsGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private BaseSystem _system = new();
    
    public EcsGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var tex = Content.Load<Texture2D>(@"up");
        var player1 = new Player(_system, new Vector2(200,200), tex);
        var player2 = new Player(_system, new Vector2(400,200), tex);
        
        _system.RegisterEntity(player1);
        _system.RegisterEntity(player2);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _system.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _system.Draw(gameTime, _spriteBatch);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}

public class Component
{
    public Entity entity;
}

public interface IDrawableComponent
{
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}

public interface IUpdateableComponent
{
    void Update(GameTime gameTime);
}

public class Transform : Component, IUpdateableComponent
{
    public Vector2 Position = Vector2.Zero;
    public Vector2 Scale = Vector2.One;
    public float Rotation = 0.0f;
    public void Update(GameTime gameTime)
    {
        
    }
}

public class Entity
{
    private BaseSystem _system;
    
    public int Id { get; set; }
    public List<Component> components = new();
    public bool isAlive = true;

    public Entity(BaseSystem system)
    {
        _system = system;
    }
    
    public void AddComponent(Component component)
    {
        components.Add(component);
        component.entity = this;
        _system.RegisterComponent(component);
    }

    public void RemoveComponent<T> () where T : Component
    {
        foreach (var comp in components.OfType<T>())
        {
            _system.DeregisterComponent(comp);
        }
        
        components.RemoveAll(x => x.GetType() == typeof(T));
    }

    public void RemoveComponents()
    {
        foreach (var comp in components)
        {
            _system.DeregisterComponent(comp);
        }
        components.Clear();
    }

    public T GetComponent<T>() where T : Component
    {
        return (T)components.Find(x => x.GetType() == typeof(T));
    }
}

public class Player : Entity
{
    public Player(BaseSystem system, Vector2 startPosition, Texture2D texture) : base(system)
    {
        Transform transform = new Transform();
        transform.Position = startPosition;
        AddComponent(transform);

        Sprite sprite = new Sprite();
        sprite.texture = texture;
        AddComponent(sprite);
        
        AddComponent(new KeyboardMove());
        AddComponent(new OutOfBoundsDestroy());
    }
}

public class KeyboardMove : Component, IUpdateableComponent
{
    public void Update(GameTime gameTime)
    {
        Transform t = entity.GetComponent<Transform>();
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            t.Position.X -= 10;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            t.Position.X += 10;
        }
    }
}

public class OutOfBoundsDestroy : Component, IUpdateableComponent
{
    public void Update(GameTime gameTime)
    {
        Transform t = entity.GetComponent<Transform>();

        if (t.Position.X < 0)
        {
            entity.isAlive = false;
        }
    }
}

public class Sprite : Component, IDrawableComponent
{
    public Texture2D texture;

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Transform t = entity.GetComponent<Transform>();
        spriteBatch.Draw(texture, t.Position, Color.White);
    }
}


public class BaseSystem
{
    public List<Component> allComponnents = new();
    public List<Entity> entities = new();

    public void RegisterEntity(Entity entity)
    {
        entities.Add(entity);
    }
    
    public void RegisterComponent(Component component)
    {
        allComponnents.Add(component);
    }

    public void DeregisterComponent(Component component)
    {
        allComponnents.Remove(component);
    }

    public void DeregisterAllComponents()
    {
        allComponnents.Clear();
    }

    public void DeregisterAllComponents<T>() where T : Component
    {
        allComponnents.RemoveAll(x => x.GetType() == typeof(T));
    }

    public void Update(GameTime gameTime)
    {

            var toKill = entities.FindAll(x => !x.isAlive);
            foreach (var ent in toKill)
            {
                ent.RemoveComponents();            
                entities.Remove(ent);
            }

        
        foreach (var component in allComponnents.OfType<IUpdateableComponent>())
        {
            component.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var component in allComponnents.OfType<IDrawableComponent>())
        {
            component.Draw(gameTime, spriteBatch);
        }
    }
}

