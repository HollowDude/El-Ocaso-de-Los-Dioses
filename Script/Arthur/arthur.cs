using Godot;
using System;


public partial class arthur : CharacterBody2D
{
    
    public float Speed = 300.0f;
	public float JumpVelocity = -100.0f;
	Vector2 velocity, direction;
	AnimationPlayer anim;
	AnimatedSprite2D Art;
	Timer WeaponOn;
	Timer IsAction;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	players_glob ArtGlobs;

    public override void _Ready(){
		
		ArtGlobs = GetNode<players_glob>("/root/PlayersGlob");
		anim = GetNode("Animations") as AnimationPlayer;
		Art = GetNode("Sprite") as AnimatedSprite2D;
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

			if(!(anim.CurrentAnimation.Equals("Fall")) || !(anim.CurrentAnimation.Equals("Dash"))){
				anim.Play("Fall");
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

		if (direction == Vector2.Zero){
			
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		}

		/*if(@event.IsActionPressed("Shield")){

			Speed = 120.0f;
			if(!(anim.CurrentAnimation.Equals("Shield")) || !(anim.CurrentAnimation.Equals("WalkShield"))){
				anim.Play("Shield");
			}

		}

		while(@event.IsActionPressed("Shield")){
			
		}*/

		if(@event.IsActionPressed("Jump") && IsOnFloor()){

			ArtGlobs.EnMovArt = true;
			velocity.Y = JumpVelocity;
			anim.Play("Jump");
			Velocity = velocity;

		}

		if(Input.IsActionPressed("Crouch") && IsOnFloor() && velocity == Vector2.Zero){

			ArtGlobs.EnMovArt = false;

			if(!(anim.CurrentAnimation.Equals("AtackCrouch"))){
				anim.Play("Crouch");
			}

		}
		if(Input.IsActionJustPressed("Atack") && anim.CurrentAnimation.Equals("Crouch")){

			anim.Play("AtackCrouch");
			ArtGlobs.Action = true;
			IsAction.Start();
			ArtGlobs.WeaponOnHand = true;
			WeaponOn.Start();

		}

		if(@event.IsActionPressed("Atack")  && !(anim.CurrentAnimation.Equals("AtackCrouch"))){

			anim.Play("AtackSide");
			ArtGlobs.Action = true;
			IsAction.Start();
			ArtGlobs.WeaponOnHand = true;
			WeaponOn.Start();

		}

		if(@event.IsActionPressed("HardAtack")){

			anim.Play("AtackUp");
			ArtGlobs.Action = true;
			IsAction.Start();
			ArtGlobs.WeaponOnHand = true;
			WeaponOn.Start();

		}

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
