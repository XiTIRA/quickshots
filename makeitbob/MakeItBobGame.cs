using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace makeitbob;

// Assets used:
// https://www.kenney.nl/assets/background-elements
// Shoutout to Kenney for this nice moon texture

/// <summary>
/// The game's base class. The platform code will hand controll to an instance of this object
/// </summary>
public class MakeItBobGame : Game
{
    // variables to store the Graphics Device and Sprite Batch.
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Below is where the rest of our game variables lives.

    // Add variables to store our texture and positions (We'll be drawing them three times)
    private Texture2D _texture;
    private Vector2 _position1;
    private Vector2 _position2;
    private Vector2 _position3;

    /// <summary>
    /// The game's constructor method.
    /// </summary>
    public MakeItBobGame()
    {
        // Keep these two lines, nothing really works without them.
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
     
        IsMouseVisible = true;
    }

    /// <summary>
    /// The game's initialize method.
    /// </summary>
    protected override void Initialize()
    {
        // Keep me, the base class handles things like calling LoadContent()!
        base.Initialize();
    }
    
    /// <summary>
    /// The game's load content method. Most - if not all - content should be loaded and parsed here.
    /// </summary>
    protected override void LoadContent()
    {
        // Keep me too! I help with batching the draw calls
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Here we load the texture (moon.png) from the Content folder
        _texture = Content.Load<Texture2D>("moon");

        // Set the initial position of the texture relative to the screen size

        // Nudge to the left (25% of the screen width)
        _position1 = new Vector2(_graphics.PreferredBackBufferWidth * .25f, _graphics.PreferredBackBufferHeight / 2.0f);

        // Center (50% of the screen width)
        _position2 = new Vector2(_graphics.PreferredBackBufferWidth * .50f, _graphics.PreferredBackBufferHeight / 2.0f);

        // Nudge to the right (75% of the screen width)
        _position3 = new Vector2(_graphics.PreferredBackBufferWidth * .75f, _graphics.PreferredBackBufferHeight / 2.0f);
    }

    /// <summary>
    /// The game's update method. Most - if not all - game logic and update methods should be done here.
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Using a sine curve against the total game time results in a smooth transition
        float y1 = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);
        _position1.Y += y1;

        // Experiment with different curves to see what they look like
        float y2 = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds);
        _position2.Y += y2;

        // Adjust the result / modify the game time to change the amplitude / speed
        float y3 = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds*5.0f)*5.0f;
        _position3.Y += y3;

        base.Update(gameTime);
    }

    /// <summary>
    /// The game's draw method. All draw calls must come through this method and should not include any logic not related to draw calls.
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
        // Resets the canvas to a single color, effectively clearing the last frame
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Always start a sprite batch with SpriteBatch.Begin()
        _spriteBatch.Begin();

        // Draw the texture at the set positions
        // Note here we are reusing the texture, in something more complex you'll probably encapsulate this in a class
        _spriteBatch.Draw(
            _texture,
            _position1,
            null,
            Color.White,
            0.0f,
            new Vector2(_texture.Width/2f, _texture.Height/2f), // Center the sprite draw call
            1.0f,
            SpriteEffects.None,
            0.0f);

        _spriteBatch.Draw(
            _texture,
            _position2,
            null,
            Color.White,
            0.0f,
            new Vector2(_texture.Width/2f, _texture.Height/2f), // Center the sprite draw call
            1.0f,
            SpriteEffects.None,
            0.0f);

        _spriteBatch.Draw(
            _texture,
            _position3,
            null,
            Color.White,
            0.0f,
            new Vector2(_texture.Width/2f, _texture.Height/2f), // Center the sprite draw call
            1.0f,
            SpriteEffects.None,
            0.0f);

        // Always end a sprite batch with SpriteBatch.End()
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    // We'll add our private game methods and classes below here

    
}
