using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace slice9;

public class Slice9Game : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Slice9 _window1;
    private Slice9 _window2;

    public Slice9Game()
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

        _window1 = new Slice9(
            Content.Load<Texture2D>("panel"),
            30,
            new Vector2(50, 50),
            new Vector2(200, 350));

        _window2 = new Slice9(
            Content.Load<Texture2D>("panel"),
            30,
            new Vector2(300, 50),
            new Vector2(400, 250));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _window1.Draw(_spriteBatch);
        _window2.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    public class Slice9
    {
       private Rectangle _topLeft;
       private Rectangle _topRight;
       private Rectangle _bottomLeft;
       private Rectangle _bottomRight;
       private Rectangle _top;
       private Rectangle _bottom;
       private Rectangle _left;
       private Rectangle _right;
       private Rectangle _center;

       private Texture2D _texture;
       private Vector2 _position;
       private Vector2 _size;
       private int _fromEdge;


       public Slice9(Texture2D texture, int fromEdge, Vector2 position, Vector2 size)
       {
           _texture = texture;
           _fromEdge = fromEdge;

           int width = _texture.Width;
           int height = _texture.Height;

           _position = position;
           _size = size;

           _topLeft = new Rectangle(0,0,fromEdge,fromEdge);
           _top = new Rectangle(fromEdge,0,width-fromEdge*2,fromEdge);
           _topRight = new Rectangle(width-fromEdge,0,fromEdge,fromEdge);
           _left = new Rectangle(0,fromEdge,fromEdge,height-fromEdge*2);
           _center = new Rectangle(fromEdge,fromEdge,width-fromEdge*2,height-fromEdge*2);
           _right = new Rectangle(width-fromEdge,fromEdge,fromEdge,height-fromEdge*2);
           _bottomLeft = new Rectangle(0,height-fromEdge,fromEdge,fromEdge);
           _bottom = new Rectangle(fromEdge,height-fromEdge,width-fromEdge*2,fromEdge);
           _bottomRight = new Rectangle(width-fromEdge,height-fromEdge,fromEdge,fromEdge);
       }

       public void Draw(SpriteBatch spriteBatch)
       {
            spriteBatch.Draw(
               _texture,
               new Rectangle((int)_position.X,(int)_position.Y,_fromEdge,_fromEdge),
               _topLeft,
               Color.White
           );
           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_size.X-_fromEdge),(int)_position.Y,_fromEdge,_fromEdge),
               _topRight,
               Color.White
           );

           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X),(int)(_position.Y+_size.Y-_fromEdge),_fromEdge,_fromEdge),
               _bottomLeft,
               Color.White
           );
           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_size.X-_fromEdge),(int)(_position.Y+_size.Y-_fromEdge),_fromEdge,_fromEdge),
               _bottomRight,
               Color.White
           );

           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X),(int)(_position.Y+_fromEdge),_fromEdge,(int)(_size.Y-(_fromEdge*2))),
               _left,
               Color.White
           );
           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_size.X-_fromEdge),(int)(_position.Y+_fromEdge),_fromEdge,(int)(_size.Y-(_fromEdge*2))),
               _right,
               Color.White
           );

           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_fromEdge),(int)(_position.Y),(int)(_size.X-(_fromEdge*2)),_fromEdge),
               _top,
               Color.White
           );
           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_fromEdge),(int)(_position.Y+_size.Y-_fromEdge),(int)(_size.X-(_fromEdge*2)),_fromEdge),
               _bottom,
               Color.White
           );

           spriteBatch.Draw(
               _texture,
               new Rectangle((int)(_position.X+_fromEdge),(int)(_position.Y+_fromEdge),(int)(_size.X-(_fromEdge*2)),(int)(_size.Y-(_fromEdge*2))),
               _center,
               Color.White
           );
       }
    }
}