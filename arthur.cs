using Godot;
using System;


public partial class arthur : CharacterBody2D
{
    
    public float Speed = 300.0f, JumpVelocity = -400.0f, gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	Vector2 velocity, direction;
	AnimationPlayer anim;
	AnimatedSprite2D Art;
	Timer WeaponOn,  IsAction;
	Area2D Sword, Palo, Axe, Shield;
	players_glob ArtGlobs;
	weapons WeaponsGlobs;
	

    public override void _Ready(){
		
		ArtGlobs = GetNode<players_glob>("/root/PlayersGlob");
		WeaponsGlobs = GetNode<weapons>("/root/Weapons");
		anim = GetNode("Animations") as AnimationPlayer;
		Art = GetNode("Sprite") as AnimatedSprite2D;
		Sword = GetNode("Weapons/Sword") as Area2D;
		Palo = GetNode("Weapons/Palo") as Area2D;
		Axe = GetNode("Weapons/Axe") as Area2D;
		Shield = GetNode("Weapons/Shield") as Area2D;
		WeaponOn = GetNode<Timer>("WeaponOn");
		WeaponOn.Timeout += () => ArtGlobs.WeaponOnHand = false;
		IsAction = GetNode<Timer>("IsAction");
		IsAction.Timeout += () => ArtGlobs.Action = false;

	}
	public override void _PhysicsProcess(double delta)
    {
       
		velocity = Velocity;

		if(!IsOnFloor()){

			velocity.Y += gravity * (float)delta;
			ArtGlobs.EnMovArt = true;

			if(!(anim.CurrentAnimation.Equals("Fall")) || !(anim.CurrentAnimation.Equals("Dash")) || !(anim.CurrentAnimation.Equals("AtackSide"))){
				anim.Play("Fall");
			}
			if(Input.IsActionPressed("Atack") && !(anim.CurrentAnimation.Equals("AtackSide"))){
				anim.Play("AtackSide");
			}

		}

		if(IsOnFloor()){

			if(direction == Vector2.Left){
				ArtGlobs.EnMovArt = true;
				ArtGlobs.DerArt = false;
				if(!(anim.CurrentAnimation.Equals("Slide"))){
					velocity.X = -Speed;
					Art.FlipH = true;
					anim.Play("Run");
				}
			}

			if(direction == Vector2.Right){
				ArtGlobs.EnMovArt = true;
				ArtGlobs.DerArt = true;
				if(!(anim.CurrentAnimation.Equals("Slide"))){
					velocity.X = Speed;
					Art.FlipH = false;
					anim.Play("Run");
				}
			}

		}

		if(velocity == Vector2.Zero && !(anim.CurrentAnimation.Equals("AtackSide")) && !(anim.CurrentAnimation.Equals("AtackUp")) && !(anim.CurrentAnimation.Equals("Crouch")) && !(anim.CurrentAnimation.Equals("Slide")) && !(anim.CurrentAnimation.Equals("Crouch")) && !(anim.CurrentAnimation.Equals("AtackCrouch"))){	
			
			ArtGlobs.EnMovArt = false;
			anim.Play("Idle");
		
		}

		Velocity = velocity;
		MoveAndSlide();	

		}

	public override /*async*/ void _Input(InputEvent @event){	

		direction = Input.GetVector("Izq", "Der", "ui_up", "ui_down");

		//Idle
		if (direction == Vector2.Zero){
			
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		}


		//Shield <<<<<<<<<<<<<<<<<<<< Pendiente
		if(Input.IsActionPressed("Shield") || (Input.IsActionPressed("Shield") && direction == Vector2.Left) || (Input.IsActionPressed("Shield") && direction == Vector2.Right)){

			Speed = 120.0f;

			if(!(anim.CurrentAnimation.Equals("Shield")) || !(anim.CurrentAnimation.Equals("WalkShield"))){
				anim.Play("Shield");
			}

		}else{Speed = 300.0f;}

		//Jump
		if(Input.IsActionPressed("Jump") && IsOnFloor()){

			ArtGlobs.EnMovArt = true;
			velocity.Y = JumpVelocity;
			anim.Play("Jump");
			Velocity = velocity;

		}

		//Agachado
		if(Input.IsActionPressed("Crouch") && IsOnFloor() && velocity == Vector2.Zero){

			ArtGlobs.EnMovArt = false;

			if(!(anim.CurrentAnimation.Equals("AtackCrouch"))){
				anim.Play("Crouch");
			}

		}

		//Atack-Agachado
		if(Input.IsActionJustPressed("Atack") && anim.CurrentAnimation.Equals("Crouch")){

			anim.Play("AtackCrouch");
			ArtGlobs.Action = true;
			IsAction.Start();
			ArtGlobs.WeaponOnHand = true;
			WeaponOn.Start();

		}

		//AtackNormal
		if(@event.IsActionPressed("Atack")  && !(anim.CurrentAnimation.Equals("AtackCrouch"))){

			anim.Play("AtackSide");
			ArtGlobs.Action = true;
			IsAction.Start();
			ArtGlobs.WeaponOnHand = true;
			WeaponOn.Start();

		}

		//Change <<<<<<<<Por acabar
		if(@event.IsActionPressed("Change") && !(anim.CurrentAnimation.Equals("AtackCrouch")) && !(anim.CurrentAnimation.Equals("Atack")) && WeaponsGlobs.Weapons != null){
			GD.Print(WeaponsGlobs.Weapons[0]);
			if(WeaponsGlobs.Pos >= 0 && WeaponsGlobs.Pos < 3){

				WeaponsGlobs.NextW();
				CollisionsChange();

			}else{

				WeaponsGlobs.ResetW();
				CollisionsChange();

			}
		}

		//Dash
		if(@event.IsActionPressed("Dash")){

			ArtGlobs.EnMovArt = true;
			//ArtGlobs.Action = true;
			//IsAction.Start();
			//ArtGlobs.WeaponOnHand = false;
			Tween tween = GetTree().CreateTween();
			
			if(!IsOnFloor()){

				anim.Play("Dash");
				
				if(Art.FlipH){
					tween.TweenProperty(GetNode("."), "position", Position + new Vector2(-200, 0) , 0.2);
				}else{tween.TweenProperty(GetNode("."), "position", Position + new Vector2(200, 0) , 0.2);}
			
			}else{

				anim.Play("Slide");

				if(Art.FlipH){
					tween.TweenProperty(GetNode("."), "position", Position + new Vector2(-190, 0) , 0.3);
				}else{tween.TweenProperty(GetNode("."), "position", Position + new Vector2(190, 0) , 0.3);}

			}
			
		}

		Velocity = velocity;
		
	}

	public void CollisionsChange(){

		if(WeaponsGlobs.Pos.Equals(0)){
			
			Sword.ProcessMode = ProcessModeEnum.Disabled;
			Palo.ProcessMode = ProcessModeEnum.Disabled;
			Axe.ProcessMode = ProcessModeEnum.Disabled;
			Shield.ProcessMode = ProcessModeEnum.Disabled;
		
		}
		if(WeaponsGlobs.Pos.Equals(1)){
			
			Sword.ProcessMode = ProcessModeEnum.Disabled;
			Palo.ProcessMode = ProcessModeEnum.Inherit;
			Axe.ProcessMode = ProcessModeEnum.Disabled;
			Shield.ProcessMode = ProcessModeEnum.Disabled;

		}
		if(WeaponsGlobs.Pos.Equals(2)){

			Sword.ProcessMode = ProcessModeEnum.Inherit;
			Palo.ProcessMode = ProcessModeEnum.Disabled;
			Axe.ProcessMode = ProcessModeEnum.Disabled;
			Shield.ProcessMode = ProcessModeEnum.Disabled;

		}
		if(WeaponsGlobs.Pos.Equals(3)){

			Sword.ProcessMode = ProcessModeEnum.Disabled;
			Palo.ProcessMode = ProcessModeEnum.Disabled;
			Axe.ProcessMode = ProcessModeEnum.Inherit;
			Shield.ProcessMode = ProcessModeEnum.Disabled;

		}
	}

}



/*Dats Imp:
-await ToSignal(GetTree().CreateTimer(0.5), "timeout");
*/
/*
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⣤⡀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣾⡿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⣹⣿⣿⣆⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣶⠆⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⡆⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⣿⣿⣿⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⡇⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣿⣁⣤⣤⣴⣶⣶⣤⣤⣄⣀⠀⠀⠀⣸⣿⣿⣿⡇⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⣦⣴⣿⣿⣿⣿⠁⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⡿⣿⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⠟⠁⠀⠹⣿⣿⡟⣰⠋⠀⠀⠈⣿⣿⣿⣿⣿⠟⠁⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠠⢼⣿⠀⠀⠀⢀⣿⣿⣇⣇⠀⠀⠀⠀⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣦⣀⣠⣾⣿⣿⣿⣿⣤⣀⣠⣾⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠻⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⢀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⣧⣀⣤⣴⣿⣿⠇⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠛⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⠀⠀⠀⣀⣴⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⠀⠀⣠⣴⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀
                                ⠀⢠⣾⣿⠟⠉⣸⣿⣿⣿⠟⠉⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣦⣄⡀⠀⠀⠀
                                ⢰⣿⡿⠁⠀⠀⣿⣿⡿⠁⣠⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⡀⠀
                                ⣾⣿⠁⠀⠀⠀⣿⣿⣷⣿⣿⠿⠛⠉⣿⣿⣿⠿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⠛⠛⠻⠈⠙⠀
                                ⢻⣿⡀⠀⢠⣾⣿⣿⡟⠉⠀⠀⠀⠀⠹⣿⣿⣇⠈⠻⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣦⣤⣤⡾
                                ⠘⢿⡇⢠⡿⠋⢹⣿⡇⠀⠀⠀⠀⠀⠀⠹⣿⣿⡆⠀⠀⠉⠻⣿⣿⡿⠿⠿⣿⣿⣷⡉⠙⢿⣿⣟⠛⠉⠁⠀
                                ⠀⢘⡿⠀⠀⠀⠀⣿⡇⠀⠀⠀⠀⠀⠀⠀⠘⢿⣿⡄⠀⠀⠀⢹⣿⠀⠀⠀⠀⠈⠹⣷⠀⠀⠙⢿⣆⣀⣀⠀
                                ⠀⠈⠁⠀⠀⠀⠀⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡇⠀⠀⠀⠘⠿⣦⠀⠀⠀⠀⠀⠛⠀⠀⠀⠀⠉⠉⠁⠀
                                ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
                             By:HollowDude...
*/
