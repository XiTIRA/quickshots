using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace animation;

// Assets used:
// https://pixelfrog-assets.itch.io/pixel-adventure-1
// Shoutout to Pixel Frog for the awesome Ninja Frog

/// <summary>
/// The game's base class. The platform code will hand controll to an instance of this object
/// </summary>
public class AnimationGame : Game
{
    // variables to store the Graphics Device and Sprite Batch.
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Below is where the rest of our game variables lives.
    
    // List to store our textures for an animation
    private List<Texture2D> _animIndividualFrames = new List<Texture2D>();
    // The current frame texture to draw
    private Texture2D _currentTexture;
    // The animations position
    private Vector2 _animationPosition;
    
    // Variables to hold our sprite sheet animations
    private SpriteSheetAnimation _frogIdle;
    private SpriteSheetAnimation _frogRun;
    
    // Variables to track the current frame and frame time
    private float _frameCountdown;
    private int _currentFrame;
    private const float SecondsPerFrame = .2f;
    
    /// <summary>
    /// The game's constructor method.
    /// </summary>
    public AnimationGame()
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
        // Keep me too! I help with batching the draw calls.
        _spriteBatch = new SpriteBatch(GraphicsDevice);
     
        // Grab the screen width to help with positioning
        int screenWidth = _graphics.PreferredBackBufferWidth;
        int yPos = _graphics.PreferredBackBufferHeight / 2;
        
        // Read in our frame textures. We save them in a list, so a local variable is fine.
        Texture2D frogDown = Content.Load<Texture2D>("Down");
        Texture2D frogUp = Content.Load<Texture2D>("Up");

        // And add them to our frame list
        _animIndividualFrames.Add(frogUp);
        _animIndividualFrames.Add(frogDown);
        
        // And give it a position
        _animationPosition = new Vector2((float)(screenWidth * .25),yPos);
        
        // Then we need to set the first frame
        _currentTexture = frogUp;
        _currentFrame = 0;
        
        // Lets populate our sprite sheet animation variables
        _frogIdle = new SpriteSheetAnimation(
            Content.Load<Texture2D>("Idle"), 
            0.2f,
            new Vector2((float)(screenWidth * .5),yPos),
            1,
            11
            );

        _frogRun = new SpriteSheetAnimation(
            Content.Load<Texture2D>("Run"), 
            0.07f,
            new Vector2((float)(screenWidth * .75),yPos),
            1,
            12
            );
    }

    /// <summary>
    /// The game's update method. Most - if not all - game logic and update methods should be done here.
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update our animation.
        FlipBookAnimationSeperateTextures(gameTime);

        // And our sprite sheet animations
        _frogRun.Update(gameTime);
        _frogIdle.Update(gameTime);
        
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

        // Draw the current texture of the animation
        _spriteBatch.Draw(_currentTexture, _animationPosition, Color.White);
        
        // And let our sprite sheet animations handle their own draws
        _frogIdle.Draw(_spriteBatch,gameTime);
        _frogRun.Draw(_spriteBatch,gameTime);
        
        // Always end a sprite batch with SpriteBatch.End()
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    // We'll add our private game methods and classes below here
    
    /// <summary>
    /// Update code for the separate texture flip book animation update.
    /// </summary>
    /// <param name="gametime">The GameTime object passed in from the Update() loop.</param>
    private void FlipBookAnimationSeperateTextures(GameTime gameTime)
    {
        // Reduce the frame timer by the elapsed number of seconds.
        _frameCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        // If the timer is complete...
        if (_frameCountdown <= 0.0f)
        {
            // Reset the frame timer.
            _frameCountdown = SecondsPerFrame;

            // Increase the frame index, and roll back to the first (0) if we've reached the end.
            _currentFrame++;
            if (_currentFrame > _animIndividualFrames.Count-1) // -1 is important, we check 0->n-1 instead of 1->n.
            {
                _currentFrame = 0;
            }
            
            // Set the current texture (frame) to the current frame index.
            _currentTexture = _animIndividualFrames[_currentFrame];
        }
    }

    /// <summary>
    /// A simple class to keep our sprite sheet animation data.
    /// </summary>
    public class SpriteSheetAnimation
    {
        // The source spritesheet.
        private Texture2D _texture;
        
        // We'll let Animation track its own frames.
        private float _secondsPerFrame;
        private float _frameTimer;
        private int _currentFrame = 0;
        private Vector2 _position;
        
        // To reduce calculations, we'll save the list of rectangles.
        private List<Rectangle> _sourceFrames = new List<Rectangle>();

        // A constructor for the animation to ensure that everything is set up correctly.
        public SpriteSheetAnimation(Texture2D texture, float secondsPerFrame, Vector2 position, int rows, int cols)
        {
            // Store the texture and seconds per frame
            _texture = texture;
            _secondsPerFrame = secondsPerFrame;
            _frameTimer = secondsPerFrame;
            _position = position;
            
            // Calculate individual frame size.
            int width = _texture.Width / cols;
            int height = _texture.Height / rows;
            
            // Split the texture by rows and columns.
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    Rectangle singleFrame = new Rectangle(
                        col * width,  // Dividing X (horizontal) by columns.
                        row * height, // Dividing Y (vertical) by rows.
                        width,
                        height
                        );
                    // And finally add the frame to the list
                    _sourceFrames.Add(singleFrame);
                }
            }
        }

        // Since frameTime holds its own timer, we'll provide an updater here.
        // GameTime here will be provided by our main update loop
        public void Update(GameTime gameTime)
        {
            // Reduce the frame timer by elapsed seconds.
            _frameTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If the timer is complete...
            if (_frameTimer <= 0.0f)
            {
                // Reset the frame timer.
                _frameTimer = _secondsPerFrame;

                // Increase the frame index, and roll back to the first (0) if we've reached the end.
                _currentFrame++;
                if (_currentFrame > _sourceFrames.Count-1) // -1 is important, we check 0->n-1 instead of 1->n.
                {
                    _currentFrame = 0;
                }
            }
        }
        
        // Since the animation stores its own texture, we'll also give it an update method.
        // SpriteBatch and GameTime will be provided by the main Update loop.
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                _texture,      // The full texture
                _position,     // Position on canvas
                _sourceFrames[_currentFrame], // Part of the texture to draw
                Color.White    // Tint color
                );
        }
    }
}