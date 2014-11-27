using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Project3
{

    public class Enemy
    {
        //Hit points that get docked off every time player hits enemy
        public int HP { set; get; }

        //public float enemySpeed { set; get; }

        // The amount of damage the enemy puts on the player 
        public int Damage { set; get; }

        //Experience that gets added to player's experience after enemy is killed.
        public int Experience { set; get; }

        public Animation EnemyAnimation;

        //List of items that player can inherit from a dead enemy.
         List <Item> EnemyItemsList = new List<Item>();

        // The position of the enemy in relation to the top left of the screen
        public Vector2 Position;

        // The current state of the Enemy 
        public bool Active { set; get; }

        public Game1 game { set; get; }

        public Enemy(Game1 game)
        {
            this.game = game;
        }

        public Enemy()
        {

        }

        public void Initialize(Animation animation, Vector2 position)
        {
            // Loads the enemy texture
            EnemyAnimation = animation;

            // Sets the position of the enemy
            Position = position;

            // Enemy is initialized to active so that it will be updated
            Active = true;

            // Enemy's health starts at 10
            HP = 10;

            // The amount of damage the enemy can do to the player
            Damage = 2;

            //enemySpeed = 5f;

            // Each enemy has 100 experience points which can be transferred to player.
            Experience = 100;
        }

        public void Update(GameTime gameTime)
        {
            
            //Position.X -= enemySpeed;

            // Enemy's position is updated
            //EnemyAnimation.Position = Position;


            //EnemyAnimation.Update(gameTime);

            // If the enemy goes past the game screen or it's health becomes 0, the enemy is no longer active
            //if (Position.X < -Width || (Health <= 0))
            //{
            //    // This results in the enemy being removed
            //    Active = false;
            //}
        }

        //public  void Update_EnemySpeed(GameTime gameTime, Player player, Enemy enemy)
        //{
        //    enemy.enemySpeed = 5f;
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draws the animation
            EnemyAnimation.Draw(spriteBatch);
        }
    }
}
