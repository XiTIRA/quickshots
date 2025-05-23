using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace scene;

public class SceneGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SceneManager _sceneManager = new SceneManager();

    public SceneGame()
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

        _sceneManager.AddScene(new GameScene(Content, _sceneManager));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _sceneManager.GetCurrentScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _sceneManager.GetCurrentScene().Draw(_spriteBatch, gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

public interface IScene
{
    public void Load();
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}

public class SceneManager
{
    private Stack<IScene> _sceneStack = new Stack<IScene>();

    public SceneManager()
    {

    }

    public void AddScene(IScene scene)
    {
        _sceneStack.Push(scene);
    }

    public void RemoveScene()
    {
        if (_sceneStack.Count > 0)
        {
            _sceneStack.Pop();
        }
    }

    public IScene GetCurrentScene()
    {
        return _sceneStack.Peek();
    }

}

public class GameScene : IScene
{

    private ContentManager _contentManager;
    private SceneManager _sceneManager;

    private Texture2D _cactusTexture;

    public GameScene(ContentManager contentManager, SceneManager sceneManager)
    {
        _contentManager = contentManager;
        _sceneManager = sceneManager;
        Load();
    }

    public void Load()
    {
        _cactusTexture = _contentManager.Load<Texture2D>("cactus");
    }

    public void Update(GameTime gameTime)
    {

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _sceneManager.AddScene(new ExitScene(_contentManager, _sceneManager));
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(_cactusTexture,new Vector2(200,200),Color.White);
    }
}

public class ExitScene : IScene
{
    private ContentManager _contentManager;
    private SceneManager _sceneManager;
    private Texture2D _cactusTexture;

    public ExitScene(ContentManager contentManager, SceneManager sceneManager)
    {
        _contentManager = contentManager;
        _sceneManager = sceneManager;
        Load();
    }

    public void Load()
    {
        _cactusTexture = _contentManager.Load<Texture2D>("cactus");
    }

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _sceneManager.AddScene(new GameScene(_contentManager, _sceneManager));
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(_cactusTexture,new Vector2(1,1),Color.White);
    }
}