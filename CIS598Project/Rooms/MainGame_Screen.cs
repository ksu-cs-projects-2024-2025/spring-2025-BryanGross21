using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Collisions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography.Xml;


namespace CIS598Project.Rooms
{
	public enum MainGame_ScreenState 
	{
		map,
		stage = 2,
		shop = 4,
		save = 6
	}
	public class MainGame_Screen : GameScreen
	{
        SpriteFont font;

		Game game;

		Player player;

		Texture2D temp;

		Song[] backgroundMusic = new Song[6];

		//map
		Texture2D[] controls = new Texture2D[5];

		Texture2D[] Fred = new Texture2D[3];

		Texture2D[] mapElements = new Texture2D[3];

		Texture2D[] TaskBar = new Texture2D[8];

		MapNode[] gameSelectors = new MapNode[9];

		Texture2D Overlay;

		//stage
		Texture2D ShowtimeOverlay;

		Texture2D StageBackground;

		Texture2D[] staticProps = new Texture2D[11];

		Texture2D[] fan = new Texture2D[2];

		int fanAnimationFrame = 0;

		double fanAnimationTimer = 0;

		Texture2D[] traffic = new Texture2D[3];

		int trafficAnimationFrame = 0;

		double trafficAnimationTimer = 0;

		Texture2D monitor;

		Rectangle monitorDrawing = new(0, 0, 384, 384);

		Texture2D[] curtain = new Texture2D[7];

		int curtainPosition = 0;

		double curtainAnimationTimer = 0;

		bool curtainRelease = false;

		Texture2D[] characterProps = new Texture2D[3];

		Texture2D[] BonPowerOff = new Texture2D[2];

		Texture2D[] FredPowerOff = new Texture2D[2];

		Texture2D[] ChicaPowerOff = new Texture2D[2];

		Texture2D[] BonPowerOn = new Texture2D[2];

		double powerAnimationTimer;

		int powerAnimationFrame = 0;

		Texture2D[] FredPowerOn = new Texture2D[2];

		Texture2D[] ChicaPowerOn = new Texture2D[2];

		Texture2D[] BonPerform = new Texture2D[3];

		Texture2D[] FredPerform = new Texture2D[3];

		Texture2D[] ChicaPerform = new Texture2D[3];

		double performAnimationTimer;

		int performAnimationFrame = 0;

		float songDuration = 0f;
		float currentTime = 0f;

		//Shop
		public Texture2D[][] items = new Texture2D[3][];

		public Texture2D arrow;

		public Texture2D shopBackground;

		SoundEffect purchase;

		BoundingRectangle[][] itemsB = new BoundingRectangle[3][];

		BoundingRectangle[] arrows = new BoundingRectangle[2];

		SoundEffect sbTalk;

		//The current row of items we are in
		int currentRow = 0;

		/// <summary>
		/// Retrieves the script to show the description, name, and controls for each minigame
		/// </summary>
		MinigameDictionary messages;

        BoundingRectangle mouse = new(0, 0, 64, 64);

        double fredAnimationTimer = 0;

		int fredAnimationCount = 0;

		bool showFredbear = false;

		bool preventMove = false; //Prevents Freddy from moving until he reaches his next position

		//Shows the current minigames tutorial
		bool tutorialShow = false;

		bool isShowtime = false;

		bool isCredits = false;

		/// <summary>
		/// 0 - 3 are for the screens, 4 is for the fredbear helper
		/// </summary>
		BoundingRectangle[] selections = new BoundingRectangle[5];

		int nodePosition = 0; //The node that the player is on.

		Texture2D[] springBonnie = new Texture2D[30];

		Vector2 springBonniePosition;

		int springBonnieFrame;

		double springBonnieTimer;

		bool reachedOtherSide = false;

		Vector2 fredPosition;

		ContentManager _content;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

        private MouseState pastMousePosition;

        private MouseState currentMousePosition;


		MainGame_ScreenState state = MainGame_ScreenState.map;

		MainGame_ScreenState pastState = MainGame_ScreenState.map;

		MainGame_ScreenState stateToGoTo;

		SoundEffect showtimeIntro;

		SoundEffectInstance intro;

		SoundEffect showtimeOutro;

		SoundEffectInstance outro;

		SoundEffect saving;

		SoundEffect saved;

        StringBuilder sr = new();

        //Timer to simulate saving
        double saveTimer = 0;

		bool isSaving;

		bool hasSaved;

		SaveClass saver = new();

		int currentGame = 0;

		public MainGame_Screen(Player player, Game game) 
		{
			this.game = game;
			this.player = player;
			game.IsMouseVisible = true;
			game.Window.Title = "Fredbear and Friends: Arcade";

			gameSelectors[0] = new(0, new Vector2(250, 250), game, player);
            gameSelectors[1] = new(1, new Vector2(450, 250), game, player);
			gameSelectors[2] = new(2, new Vector2(450, 450), game, player);
            gameSelectors[3] = new(3, new Vector2(650, 450), game, player);
            gameSelectors[4] = new(4, new Vector2(650, 650), game, player);
			gameSelectors[5] = new(5, new Vector2(450, 650), game, player);
            gameSelectors[6] = new(6, new Vector2(250, 650), game, player);
            gameSelectors[7] = new(7, new Vector2(250, 450), game, player); 
			gameSelectors[8] = new(8, new Vector2(250, 50), game, player);

			selections[0] = new(0, 1026, 192, 54);
			selections[1] = new(248, 1026, 150, 54);
			selections[2] = new(454, 1026, 150, 54);
			selections[3] = new(652, 1026, 150, 54);

			fredPosition = new Vector2(250, 250);

			items[0] = new Texture2D[13];
			items[1] = new Texture2D[5];
			items[2] = new Texture2D[3];

			messages = new();
        }


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			foreach (MapNode node in gameSelectors) 
			{
				if (node != null) 
				{
					node.LoadContent(_content);
				}
			}

			messages.LoadContent(_content);

			//The music for each screen
			backgroundMusic[0] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/8BitTravel(Map)");
            backgroundMusic[1] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/Broken8Bit(Save)");
            backgroundMusic[2] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/Thank_You_For_Your_Patience_2_(shop)");
            backgroundMusic[3] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/stage(no_act)");
            backgroundMusic[4] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/stage(acting)");
			backgroundMusic[5] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/Musicbox_ending");

			TaskBar[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/map");
            TaskBar[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/map_select_fredbear");
            TaskBar[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/stage");
            TaskBar[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/stageFredb");
            TaskBar[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/store");
            TaskBar[5] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/storeFredb");
            TaskBar[6] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/save");
            TaskBar[7] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/saveFredb");

			mapElements[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/horizonal_path");
            mapElements[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/vertical_path");
            mapElements[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/coin");

			Fred[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_1");
            Fred[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_2");
            Fred[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_3");

			controls[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/w_key");
            controls[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/a_key");
            controls[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/s_key");
            controls[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/d_key");
            controls[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/e_key");

			Overlay = _content.Load<Texture2D>("gameSelectionOverlay");

			font = _content.Load<SpriteFont>("MiniGame_Font");

			StageBackground = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Stage_background");

			staticProps[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Stage");
			staticProps[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Sanitation_Station");
			staticProps[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Paper_Pals");
			staticProps[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Table");
			staticProps[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Table_with_hats");
			staticProps[5] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/present");
			staticProps[6] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/childrens_drawings");
			staticProps[7] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/celebrate_poster");
			staticProps[8] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/fazerblast");
			staticProps[9] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Confetti_floor");
			staticProps[10] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Gumball_Swivelhands");

			fan[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/fan_1");
			fan[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/fan_2");

			traffic[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/traffic_light_1");
			traffic[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/traffic_light_2");
			traffic[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/traffic_light_3");

			characterProps[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/guitar_upright");
			characterProps[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Microphone");
			characterProps[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/mr_cupcake");

			monitor = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/Showtime_monitor");

			BonPowerOff[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_no_prop");
			BonPowerOff[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_Power_Off");
			BonPowerOn[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_power_on_1");
			BonPowerOn[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_power_on_2");
			BonPerform[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_perform_1");
			BonPerform[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_perform_2");
			BonPerform[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Bon/Bonnie_perform_3");

			FredPowerOff[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_no_prop");
			FredPowerOff[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_Power_Off");
			FredPowerOn[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_power_on_1");
			FredPowerOn[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_power_on_2");
			FredPerform[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_perform_1");
			FredPerform[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_perform_2");
			FredPerform[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Fred/Freddy_perform_3");

			ChicaPowerOff[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_no_prop");
			ChicaPowerOff[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_Power_Off");
			ChicaPowerOn[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_power_on_1");
			ChicaPowerOn[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_power_on_2");
			ChicaPerform[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_perform_1");
			ChicaPerform[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_perform_2");
			ChicaPerform[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Chica/Chica_perform_3");

			curtain[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains1");
			curtain[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains2");
			curtain[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains3");
			curtain[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains4");
			curtain[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains5");
			curtain[5] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains6");
			curtain[6] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/Background_Props/curtains7");

			showtimeIntro = _content.Load<SoundEffect>("Desktop_Selection/Sounds/Soundeffects/stage_intro");
			showtimeOutro = _content.Load<SoundEffect>("Desktop_Selection/Sounds/Soundeffects/stage_end");

			saving = _content.Load<SoundEffect>("Desktop_Selection/Sounds/Soundeffects/saving");
			saved = _content.Load<SoundEffect>("Desktop_Selection/Sounds/Soundeffects/saved");

			items[0][0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/stage_node");
			items[0][1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/sanitation_node");
			items[0][2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/paper_node");
			items[0][3] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/table_node");
			items[0][4] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/hats_node");
			items[0][5] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/present_node");
			items[0][6] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/drawings_node");
			items[0][7] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/traffic_light");
			items[0][8] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/celebrate_node");
			items[0][9] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/fazer_node");
			items[0][10] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/fan_node");
			items[0][11] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/monitor_node");
			items[0][12] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/gumball_node");

			items[1][0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/curtains_node");
			items[1][1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/confetti_node");
			items[1][2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/guitar_node");
			items[1][3] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/mic_node");
			items[1][4] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/cupcake_node");


			items[2][0] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/bonnie_node");
			items[2][1] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/fred_node");
			items[2][2] = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/chica_node");

			arrow = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/arrow");

			arrows[0] = new BoundingRectangle(game.GraphicsDevice.Viewport.Width / 2 + 750, game.GraphicsDevice.Viewport.Height / 2 - 35, 128, 128); //Right
			arrows[1] = new BoundingRectangle(game.GraphicsDevice.Viewport.Width / 2 + 550, game.GraphicsDevice.Viewport.Height / 2 - 35, 128, 128); //Left

			shopBackground = _content.Load<Texture2D>("Desktop_Selection/Textures/Shop/Textures/Everything_else/background");

			purchase = _content.Load<SoundEffect>("Desktop_Selection/Textures/Shop/Sounds/Soundeffects/purchase");

			ShowtimeOverlay = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Overlay");

			temp = _content.Load<Texture2D>("temp");

			for (int i = 0; i < springBonnie.Length; i++)
			{
				string add = "";
				if ((i + 1) < 10)
				{
					add = ("000" + (i + 1));
				}
				if ((i + 1) >= 10)
				{
					add = ("00" + (i + 1));
				}
				springBonnie[i] = _content.Load<Texture2D>("Desktop_Selection/Textures/Stage/SB/" + add);
			}

			springBonniePosition = new(game.GraphicsDevice.Viewport.Width + 125, game.GraphicsDevice.Viewport.Height - 250);

			songDuration = backgroundMusic[4].Duration.Seconds;

			MediaPlayer.Play(backgroundMusic[(int)state]);
			MediaPlayer.IsRepeating = true;
		}



		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();

			pastState = state;

			if (!ScreenManager.Game.IsActive)
			{
				// Pause the music or stop sound effects when the game loses focus
				if (MediaPlayer.State == MediaState.Playing)
				{
					MediaPlayer.Pause();
				}
				return;
			}
			else
			{
				// Resume music if the game becomes active again
				if (MediaPlayer.State == MediaState.Paused)
				{
					MediaPlayer.Resume();
				}
			}

			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

            if (isShowtime == false)
            {
                if (tutorialShow == false)
                {
                    if (mouse.collidesWith(selections[0]))
                    {
                        if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released && state != MainGame_ScreenState.map)
                        {
                            state = MainGame_ScreenState.map;
                        }
                    }

                    if (mouse.collidesWith(selections[1]))
                    {
                        if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released && state != MainGame_ScreenState.stage)
                        {
                            state = MainGame_ScreenState.stage;
                        }
                    }

					if (mouse.collidesWith(selections[2]))
					{
						if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released && state != MainGame_ScreenState.shop)
						{
							state = MainGame_ScreenState.shop;
						}
					}

					if (mouse.collidesWith(selections[3]))
                    {
                        if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released && state != MainGame_ScreenState.save)
                        {
							stateToGoTo = state;
                            state = MainGame_ScreenState.save;
                        }
                    }
                }
            }

            if (state == MainGame_ScreenState.map && showFredbear == false)
			{
				fredUpdate();
			}

			if (state == MainGame_ScreenState.stage && showFredbear == false) 
			{
				stageUpdate(gameTime);
			}


			if (state == MainGame_ScreenState.shop && showFredbear == false)
			{
				shopUpdate();
			}

			if (state == MainGame_ScreenState.save && showFredbear == false) 
			{
				saveUpdate(gameTime);
			}


			if (pastState != state) 
			{
				if (state == MainGame_ScreenState.stage) 
				{
					MediaPlayer.Stop();
					if (player.itemsUnlocked[0][0]) 
					{
						MediaPlayer.Play(backgroundMusic[3]);
					}
				}
				if (state == MainGame_ScreenState.map) 
				{
					MediaPlayer.Play(backgroundMusic[0]);
				}
				if (state == MainGame_ScreenState.save)
				{
					MediaPlayer.Play(backgroundMusic[1]);
				}
				if (state == MainGame_ScreenState.shop)
				{
					MediaPlayer.Play(backgroundMusic[2]);
				}
			}

			mouse.X = mousePosition.X;
            mouse.Y = mousePosition.Y;
        }

		private void saveUpdate(GameTime gameTime)
		{
			if (currentKeyboardState.IsKeyDown(Keys.Y) && pastKeyboardState.IsKeyDown(Keys.Y) && isSaving == false && hasSaved == false)
			{
					isSaving = true;
			}
			else if(currentKeyboardState.IsKeyDown(Keys.N) && pastKeyboardState.IsKeyDown(Keys.N) && isSaving == false && hasSaved == false)
			{
				state = stateToGoTo;
			}
			if (isSaving)
			{
				saveTimer += gameTime.ElapsedGameTime.TotalSeconds;


				if (saveTimer > .75)
				{
                    saver.save(sr, player);
                    hasSaved = true;
                    isSaving = false;
                    saveTimer -= .75;
				}
			}
			if (hasSaved) 
			{
                saveTimer += gameTime.ElapsedGameTime.TotalSeconds;


                if (saveTimer > 1)
                {
                    hasSaved = false;
					isSaving = false;
					state = stateToGoTo;
                    saveTimer -= 1;
                }
            }
        }

		private void stageUpdate(GameTime gameTime) 
		{
			if (showFredbear == false && checkUnlocks())
			{
				if (currentKeyboardState.IsKeyDown(Keys.E) && isShowtime == false)
				{
					isShowtime = true;
				}
			}

            if (isShowtime && curtainRelease == false && curtainPosition != 6)
            {
                MediaPlayer.Stop();
                if (intro == null)
                {
                    intro = showtimeIntro.CreateInstance();
                    intro.Play();
                }
                if (intro.State != SoundState.Playing)
                {
                    curtainRelease = true;
                }
            }

            if (curtainRelease)
            {
                curtainAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (curtainAnimationTimer > .5)
                {
                    curtainPosition++;
                    if (curtainPosition == 7)
                    {
                        curtainPosition = 6;
                        curtainRelease = false;
                    }
                    curtainAnimationTimer -= .5;
                }
            }

            if (curtainPosition == 5)
            {
                MediaPlayer.Play(backgroundMusic[4]);
                MediaPlayer.IsRepeating = false;
            }

            if (curtainPosition == 6 && isCredits == false)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    isCredits = true;
                    MediaPlayer.Play(backgroundMusic[5]);
                }
            }

            if (isCredits)
            {
                if (reachedOtherSide)
                {
                    springBonniePosition.X += 10;
                }
                else
                {
                    springBonniePosition.X -= 10;
                }

                if (springBonniePosition.X <= -125 || springBonniePosition.X >= game.GraphicsDevice.Viewport.Width + 125)
                {
                    reachedOtherSide = !reachedOtherSide;
                }

                if (MediaPlayer.State != MediaState.Playing)
                {
                    if (outro == null)
                    {
                        outro = showtimeOutro.CreateInstance();
                        outro.Play();
                    }
                    if (outro.State != SoundState.Playing)
                    {
                        nodePosition = 9;
                        transitionToMinigame();
                    }
                }
            }
        }

		private void transitionToMinigame() 
		{
			foreach (var screen in ScreenManager.GetScreens())
				screen.ExitScreen();
			if (nodePosition == 0) 
			{
				ScreenManager.AddScreen(new DuckPond(game, player), null);
			}
			if (nodePosition == 1)
			{
				ScreenManager.AddScreen(new Discount_Ballpit(game, player), null);
			}
			if (nodePosition == 2)
			{
				ScreenManager.AddScreen(new BalloonBarrel(game, player), null);
			}
			if (nodePosition == 3)
			{
				ScreenManager.AddScreen(new Ballpit_Tower(game, player), null);
			}
			if (nodePosition == 4)
			{
				ScreenManager.AddScreen(new FruityMaze(game, player), null);
			}
			if (nodePosition == 5)
			{
				ScreenManager.AddScreen(new Memory_Match(game, player), null);
			}
			if (nodePosition == 6)
			{
				ScreenManager.AddScreen(new Security(game, player), null);
			}
			if (nodePosition == 7)
			{
				ScreenManager.AddScreen(new Fishing(game, player), null);
			}
			if (nodePosition == 8) 
			{
				ScreenManager.AddScreen(new Ending_Screen(game, player, true), null); //Secret Ending
			}
			if (nodePosition == 9) 
			{
				ScreenManager.AddScreen(new Ending_Screen(game, player, false), null);
			}
		}

		private void fredUpdate() 
		{
			//Handle the player's input to go to the next node
			if (preventMove == false && tutorialShow == false)
			{
				if (currentKeyboardState.IsKeyDown(Keys.A) && pastKeyboardState.IsKeyDown(Keys.A))
				{
					nodePosition--;
					if (nodePosition < 0) 
					{
						if (checkFoundSecret() == false)
						{
							nodePosition = 7;
						}
						else 
						{
							nodePosition = 8;
						}
					}
					preventMove = true;
				}
				if (currentKeyboardState.IsKeyDown(Keys.D) && pastKeyboardState.IsKeyDown(Keys.D))
				{
					nodePosition++;
					if (checkFoundSecret() == false)
					{
						if (nodePosition > 7)
						{
							nodePosition = 0;
						}
					}
					else 
					{
						if (nodePosition > 8) 
						{
							nodePosition = 0;
						}
					}
					preventMove = true;
				}
				if (currentKeyboardState.IsKeyDown(Keys.E)) 
				{
					tutorialShow = true;
					//preventMove = true; //Don't want the player to move and select a new game while a tutorial message is shown
				}
			}

			//Get ready to go to the next screen for the minigame or to exit back to the map selection
			if (tutorialShow) 
			{
				if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
				{
					transitionToMinigame();
				}
				if (currentKeyboardState.IsKeyDown(Keys.Escape))
				{
					tutorialShow = false;
				}
			}

			//Update Freddy to move to the next node
			if (preventMove) 
			{
				if (fredPosition.X != gameSelectors[nodePosition].position.X) 
				{
					if (fredPosition.X > gameSelectors[nodePosition].position.X)
					{
						fredPosition.X -= 25;
					}
					else if (fredPosition.X < gameSelectors[nodePosition].position.X) 
					{
						fredPosition.X += 25;
					}
				}
				if (fredPosition.Y != gameSelectors[nodePosition].position.Y) 
				{
					if (fredPosition.Y > gameSelectors[nodePosition].position.Y)
					{
						fredPosition.Y -= 25;
					}
					else if (fredPosition.Y < gameSelectors[nodePosition].position.Y)
					{
						fredPosition.Y += 25;
					}
				}
				if (fredPosition.X == gameSelectors[nodePosition].position.X && gameSelectors[nodePosition].position.Y == fredPosition.Y) 
				{
					preventMove = false;
				}
			}
		}

		private void shopUpdate()
		{
			if (currentRow == 0 || currentRow == 1)
			{
				if (mouse.collidesWith(arrows[0]))
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						currentRow++;
					}
				}

			}
			if (currentRow == 1 || currentRow == 2) 
			{
				if (mouse.collidesWith(arrows[1]))
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						currentRow--;
					}
				}
			}
		}

		/// <summary>
		/// Checks to see if the player has all items unlocked
		/// </summary>
		/// <returns>True or false dependent on if all items are unlocked</returns>
		private bool checkUnlocks() 
		{
			for(int i = 0; i < player.itemsUnlocked.Length; i++) 
            {
				for (int j = 0; j < player.itemsUnlocked[i].Length; j++)
				{
					if (player.itemsUnlocked[i][j] == false) 
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool checkFoundSecret()
		{
			for (int i = 0; i < player.foundSecret.Length; i++)
			{
				if (player.foundSecret[i] == false) 
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Draws the sprite using the supplied SpriteBatch
		/// </summary>
		/// <param name="gameTime">The game time</param>
		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;


			graphics.Clear(Color.Black);


			spriteBatch.Begin();


			if (state == MainGame_ScreenState.map) 
			{
				bool found = checkFoundSecret();
				spriteBatch.DrawString(font, "Movement: ", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 - 250), Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Selection: ", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0);
                spriteBatch.Draw(controls[4], new Vector2(graphics.Viewport.Width / 2 + 250, graphics.Viewport.Height / 2), Color.White);


                spriteBatch.Draw(mapElements[0], new Vector2(350, 250), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(450, 350), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(550, 450), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(650, 550), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(550, 650), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(350, 650), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(250, 550), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(250, 350), Color.White);

				if (found)
				{
					spriteBatch.Draw(mapElements[1], new Vector2(250, 150), Color.White);
				}

				foreach (MapNode node in gameSelectors)
                {
                    if (node != null)
                    {
						if (MinigameRef.secret_ending != node.referenceType)
						{
							node.Draw(gameTime, spriteBatch);
						}
						if (MinigameRef.secret_ending == node.referenceType && found) 
						{
							node.Draw(gameTime, spriteBatch);
						}
                    }
                }

				fredAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (fredAnimationTimer > .3) 
				{
                    fredAnimationCount++;
                    if (fredAnimationCount == 3) 
					{
						fredAnimationCount = 0;
					}
					fredAnimationTimer -= .3;
				}

				spriteBatch.Draw(Fred[fredAnimationCount], fredPosition, Color.White);

                spriteBatch.Draw(controls[1], new Vector2(graphics.Viewport.Width / 2 + 300, graphics.Viewport.Height / 2 - 250), Color.White);
                spriteBatch.Draw(controls[3], new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 - 260), Color.White);

				spriteBatch.DrawString(font, "Tickets: " + player.ticketAmount, Vector2.Zero, Color.White);

				if (tutorialShow) 
				{
					spriteBatch.Draw(Overlay, Vector2.Zero, Color.White);
					messages.Draw(spriteBatch, nodePosition, graphics.Viewport.Width, graphics.Viewport.Height);
				}
			}

			if (state == MainGame_ScreenState.stage)
			{
				spriteBatch.Draw(StageBackground, Vector2.Zero, Color.White);

				if (player.itemsUnlocked[1][1])
				{
					spriteBatch.Draw(staticProps[9], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][0])
				{
					spriteBatch.Draw(staticProps[0], new Vector2(775, 200), Color.White);
				}
				if (player.itemsUnlocked[1][2] && player.itemsUnlocked[2][0] == false)
				{
					spriteBatch.Draw(characterProps[0], new Vector2(775, 350), Color.White);
				}
				if (player.itemsUnlocked[1][3] && player.itemsUnlocked[2][1] == false)
				{
					spriteBatch.Draw(characterProps[1], new Vector2(1075, 590), Color.White);
				}
				if (player.itemsUnlocked[1][4] && player.itemsUnlocked[2][2] == false)
				{
					spriteBatch.Draw(characterProps[2], new Vector2(1275, 570), Color.White);
				}
				if (player.itemsUnlocked[2][0])
				{
					if (player.itemsUnlocked[0][0])
					{
						if (isShowtime == false || curtainPosition < 6 || isCredits)
						{
							if (player.itemsUnlocked[1][2])
							{
								spriteBatch.Draw(BonPowerOff[1], new Vector2(775, 300), Color.White);
							}
							else
							{
								spriteBatch.Draw(BonPowerOff[0], new Vector2(775, 300), Color.White);
							}
						}
						else
						{
							if (curtainPosition == 6)
							{
								performAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
								if (performAnimationTimer > .75)
								{
									performAnimationFrame++;
									if (performAnimationFrame == 3)
									{
										performAnimationFrame = 0;
									}
									performAnimationTimer -= .75;
								}
								spriteBatch.Draw(BonPerform[performAnimationFrame], new Vector2(775, 300), Color.White);
							}
						}
					}
				}
				if (player.itemsUnlocked[2][2])
				{
					if (player.itemsUnlocked[0][0])
					{
						if (isShowtime == false || curtainPosition < 6 || isCredits)
						{
							if (player.itemsUnlocked[1][4])
							{
								spriteBatch.Draw(ChicaPowerOff[1], new Vector2(1175, 300), Color.White);
							}
							else
							{
								spriteBatch.Draw(ChicaPowerOff[0], new Vector2(1175, 300), Color.White);
							}
						}
						else
						{
							if (curtainPosition == 6)
							{
								spriteBatch.Draw(ChicaPerform[performAnimationFrame], new Vector2(1175, 300), Color.White);
							}

						}
					}
				}
				if (player.itemsUnlocked[2][1])
				{
					if (player.itemsUnlocked[0][0])
					{
						if (isShowtime == false || curtainPosition < 6 || isCredits)
						{
							if (player.itemsUnlocked[1][3])
							{
								spriteBatch.Draw(FredPowerOff[1], new Vector2(975, 300), Color.White);
							}
							else
							{
								spriteBatch.Draw(FredPowerOff[0], new Vector2(975, 300), Color.White);
							}
						}
						else
						{
							if (curtainPosition == 6)
							{
								spriteBatch.Draw(FredPerform[performAnimationFrame], new Vector2(975, 300), Color.White);
							}
						}
					}
				}
				if (player.itemsUnlocked[1][0])
				{
					if (isShowtime == false)
					{
						spriteBatch.Draw(curtain[0], new Vector2(775, 150), Color.White);
					}
					else
					{
						spriteBatch.Draw(curtain[curtainPosition], new Vector2(775, 150), Color.White);
					}
				}
				if (player.itemsUnlocked[0][9])
				{
					spriteBatch.Draw(staticProps[8], new Vector2(100, 305), Color.White);
				}
				if (player.itemsUnlocked[0][1])
				{
					spriteBatch.Draw(staticProps[1], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][2])
				{
					spriteBatch.Draw(staticProps[2], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][6])
				{
					spriteBatch.Draw(staticProps[6], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][5])
				{
					spriteBatch.Draw(staticProps[5], new Vector2(1500, 550), Color.White);
				}
				if (player.itemsUnlocked[0][7])
				{

					if (isShowtime)
					{
						trafficAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
						if (trafficAnimationTimer > .75)
						{
							trafficAnimationFrame++;
							if (trafficAnimationFrame >= 3)
							{
								trafficAnimationFrame = 0;
							}
							trafficAnimationTimer -= .75;
						}
					}
					spriteBatch.Draw(traffic[trafficAnimationFrame], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][8])
				{
					spriteBatch.Draw(staticProps[7], new Vector2(500, 255), Color.White);
				}
				if (player.itemsUnlocked[0][10])
				{
					fanAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if (fanAnimationTimer > .25)
					{
						fanAnimationFrame++;
						if (fanAnimationFrame >= 2)
						{
							fanAnimationFrame = 0;
						}
						fanAnimationTimer -= .25;
					}
					spriteBatch.Draw(fan[fanAnimationFrame], Vector2.Zero, Color.White);
				}
				if (player.itemsUnlocked[0][12])
				{
					spriteBatch.Draw(staticProps[10], Vector2.Zero, Color.White);
				}
				springBonnieTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (springBonnieTimer > .15)
				{
					springBonnieFrame++;
					if (springBonnieFrame >= 30)
					{
						springBonnieFrame = 0;
					}
					springBonnieTimer -= .15;
				}
				//Springbonnie draw logic
				if (reachedOtherSide == false && isCredits)
				{
					spriteBatch.Draw(springBonnie[springBonnieFrame], springBonniePosition, null, Color.White, 0f, new Vector2(springBonnie[springBonnieFrame].Bounds.Width / 2, springBonnie[springBonnieFrame].Bounds.Height / 2), .5f, SpriteEffects.None, 0);
				}
				if (player.itemsUnlocked[0][3])
				{
					if (player.itemsUnlocked[0][4])
					{
						spriteBatch.Draw(staticProps[4], new Vector2(0, 737), Color.White);
						spriteBatch.Draw(staticProps[3], new Vector2(620, 737), Color.White);
						spriteBatch.Draw(staticProps[4], new Vector2(graphics.Viewport.Width - 684, 737), Color.White);
					}
					else
					{
						spriteBatch.Draw(staticProps[3], new Vector2(0, 737), Color.White);
						spriteBatch.Draw(staticProps[3], new Vector2(620, 737), Color.White);
						spriteBatch.Draw(staticProps[3], new Vector2(graphics.Viewport.Width - 684, 737), Color.White);
					}
				}
				if (player.itemsUnlocked[0][11] && player.itemsUnlocked[0][3])
				{
					if (isShowtime)
					{
						monitorDrawing = new Rectangle(0, 384, 384, 384);
					}
					spriteBatch.Draw(monitor, new Vector2(955, 937), monitorDrawing, Color.White, 0f, new Vector2(monitor.Bounds.Width / 2, monitor.Bounds.Height / 2), .75f, SpriteEffects.None, 0);
					if (checkUnlocks() && isShowtime == false)
					{
						spriteBatch.DrawString(font, "E for showtime", new Vector2(855, 737), Color.White, 0f, Vector2.Zero, .35f, SpriteEffects.None, 0);
					}
				}
				if (reachedOtherSide == true && isCredits)
				{
					spriteBatch.Draw(springBonnie[springBonnieFrame], springBonniePosition, null, Color.White, 0f, new Vector2(springBonnie[springBonnieFrame].Bounds.Width / 2, springBonnie[springBonnieFrame].Bounds.Height / 2), .5f, SpriteEffects.FlipHorizontally, 0);
				}

				if (isShowtime)
				{
					spriteBatch.Draw(ShowtimeOverlay, Vector2.Zero, Color.White);
				}

			}

			if (state == MainGame_ScreenState.shop)
			{
				spriteBatch.Draw(shopBackground, Vector2.Zero, Color.White);

				spriteBatch.DrawString(font, "Tickets:\n" + player.ticketAmount.ToString(), new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 + 100), Color.Black);
				if (currentRow == 0)
				{
					spriteBatch.DrawString(font, "Page (Props):\n" + (currentRow + 1) + "/3", new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 + 260), Color.Black);
					spriteBatch.Draw(arrow, new Vector2(graphics.Viewport.Width / 2 + 750, graphics.Viewport.Height / 2 - 35), Color.White);
				}
				if (currentRow == 1)
				{
					spriteBatch.DrawString(font, "Page (Upgr.):\n" + (currentRow + 1) + "/3", new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 + 260), Color.Black);
					spriteBatch.Draw(arrow, new Vector2(graphics.Viewport.Width / 2 + 750, graphics.Viewport.Height / 2 - 35), Color.White);
					spriteBatch.Draw(arrow, new Vector2(graphics.Viewport.Width / 2 + 550, graphics.Viewport.Height / 2 - 35), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0 );
				}
				if (currentRow == 2)
				{
					spriteBatch.DrawString(font, "Page (Anim.):\n" + (currentRow + 1) + "/3", new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 + 260), Color.Black);
					spriteBatch.Draw(arrow, new Vector2(graphics.Viewport.Width / 2 + 550, graphics.Viewport.Height / 2 - 35), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0);
				}

			}

			if (state == MainGame_ScreenState.save)
			{
				if (!isSaving && !hasSaved)
				{
					spriteBatch.DrawString(font, "Save game?", new Vector2(graphics.Viewport.Width / 2 - 250, graphics.Viewport.Height / 4), Color.White);

					spriteBatch.DrawString(font, "Y for yes, N for No", new Vector2(graphics.Viewport.Width / 2 - 250, graphics.Viewport.Height / 2), Color.White);
				}
				if (isSaving && hasSaved == false) 
				{
                    spriteBatch.DrawString(font, "Saving...", new Vector2(graphics.Viewport.Width / 2 - 250, graphics.Viewport.Height / 4), Color.White);
                }
				if (hasSaved && isSaving == false) 
				{
                    spriteBatch.DrawString(font, "Saved!", new Vector2(graphics.Viewport.Width / 2 - 250, graphics.Viewport.Height / 4), Color.White);
                }
			}

			if (showFredbear)
			{
				spriteBatch.Draw(TaskBar[(int)state + 1], Vector2.Zero, Color.White);
			}
			else
			{
				spriteBatch.Draw(TaskBar[(int)state], Vector2.Zero, Color.White);
			}


			spriteBatch.End();
		}


	}
}
