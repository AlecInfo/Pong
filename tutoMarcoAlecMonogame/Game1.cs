/*
 Auteur: Marco et Alec
 Date: 25.11.2021
 Description: création d'un tuto en faisant un Pong
*/


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace tutoMarcoAlecMonogame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont police;
        private Random rnd = new Random();

        private Texture2D textureParDefaut;

        // paddle
        int paddle1PositionY = 0;
        int paddle2PositionY = 0;
        int vitessePaddles = 7;

        // balle
        Vector2 positionBalle;
        Vector2 directionBalle;
        private Vector2 _vitesseBalle;
        private int _vitesseBalles;


        private Vector2 tailleFenetre
        {
            get { return new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight); }
        }

        void DirectionBalle()
        {
            directionBalle = new Vector2(
                rnd.Next(0, 2) == 0 ? -5 : 5,
                rnd.Next(0, 2) == 0 ? -5 : 5
            );
        }

        // score
        private int _scoreP1;
        private int _scoreP2;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            police = Content.Load<SpriteFont>("File");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            textureParDefaut = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            textureParDefaut.SetData(new Color[] { Color.White });

            paddle1PositionY = (int)tailleFenetre.Y / 2;
            paddle2PositionY = (int)tailleFenetre.Y / 2;

            positionBalle = tailleFenetre / 2;

            DirectionBalle();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kState = Keyboard.GetState();

            // input joueur 1
            if (kState.IsKeyDown(Keys.W))
            {
                paddle1PositionY -= vitessePaddles;

                if (paddle1PositionY < 0)
                    paddle1PositionY = 0;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                paddle1PositionY += vitessePaddles;

                if (paddle1PositionY > tailleFenetre.Y)
                    paddle1PositionY = (int)tailleFenetre.Y;
            }

            // input joueur 2
            if (kState.IsKeyDown(Keys.Up))
            {
                paddle2PositionY -= vitessePaddles;

                if (paddle2PositionY < 0)
                    paddle2PositionY = 0;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                paddle2PositionY += vitessePaddles;

                if (paddle2PositionY > tailleFenetre.Y)
                    paddle2PositionY = (int)tailleFenetre.Y;
            }


            // Collision balle
            if (positionBalle.Y < 0) // Haut
            {
                positionBalle.Y = 0;
                directionBalle.Y = -directionBalle.Y;
            }

            if (positionBalle.Y > tailleFenetre.Y) // Bas
            {
                positionBalle.Y = tailleFenetre.Y;
                directionBalle.Y = -directionBalle.Y;

            }

            if (positionBalle.X < 0) // Gauche
            {
                positionBalle = tailleFenetre / 2;
                DirectionBalle();
                _scoreP2 += 1;
                _vitesseBalles = 0;
            }
            else if ((positionBalle.Y < paddle1PositionY + 55 && positionBalle.Y > paddle1PositionY - 50) && positionBalle.X <= 15)
            {
                directionBalle.X = -directionBalle.X;

                if (_vitesseBalles < 5)
                {
                    _vitesseBalles += 1;
                }

            }

            if (positionBalle.X > tailleFenetre.X) // Droite
            {
                positionBalle = tailleFenetre / 2;
                DirectionBalle();
                _scoreP1 += 1;
                _vitesseBalles = 0;
            }
            else if ((positionBalle.Y < paddle2PositionY + 55 && positionBalle.Y > paddle2PositionY - 50) && positionBalle.X >= tailleFenetre.X - 15)
            {
                directionBalle.X = -directionBalle.X;


                if (_vitesseBalles < 5)
                {
                    _vitesseBalles += 1;
                }
            }

            if (directionBalle.X < 0)
                _vitesseBalle.X = -_vitesseBalles;
            else
                _vitesseBalle.X = _vitesseBalles;

            if (directionBalle.Y < 0)
                _vitesseBalle.Y = -_vitesseBalles;
            else
                _vitesseBalle.Y = _vitesseBalles;
            
            
            positionBalle += directionBalle + _vitesseBalle;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // Score
            _spriteBatch.DrawString(police, "Joueur 1", new Vector2(tailleFenetre.X / 2 - police.MeasureString("Joueur 1").X - 20, 10), Color.White);
            _spriteBatch.DrawString(police, _scoreP1.ToString(), new Vector2(tailleFenetre.X / 2 - police.MeasureString("Joueur 1").X / 2 - 20, 30), Color.White);
            _spriteBatch.DrawString(police, "Joueur 2", new Vector2(tailleFenetre.X / 2 + 20, 10), Color.White);
            _spriteBatch.DrawString(police, _scoreP2.ToString(), new Vector2(tailleFenetre.X / 2 + police.MeasureString("Joueur 2").X / 2 + 20, 30), Color.White);

            // Ligne du milieu
            _spriteBatch.Draw(textureParDefaut, new Rectangle(((int)tailleFenetre.X-3) / 2, 0, 3, (int)tailleFenetre.Y), Color.White);

            //Paddle
            _spriteBatch.Draw(textureParDefaut, new Rectangle(15, paddle1PositionY, 3, 100), null, Color.White, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(textureParDefaut, new Rectangle((int)tailleFenetre.X-15, paddle2PositionY, 3, 100), null, Color.White, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

            // Balle
            _spriteBatch.Draw(textureParDefaut, new Rectangle((int)positionBalle.X, (int)positionBalle.Y, 15, 15), null, Color.Red, 0f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
