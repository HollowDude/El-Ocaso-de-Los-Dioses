using Godot;
using System;

public partial class arthur : CharacterBody2D
{
    
    public const float Speed = 300.0f;
	public const float JumpVelocity = -250.0f;
	Vector2 velocity;
	Vector2 direction;
	AnimationPlayer anim;
	AnimatedSprite2D Art;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
    public override void _Ready(){

		anim = GetNode("Animations") as AnimationPlayer;
		Art = GetNode("Sprite") as AnimatedSprite2D;

	}
	public override void _PhysicsProcess(double delta)
    {

		velocity = Velocity;

		if (!IsOnFloor()){
			velocity.Y += gravity * (float)delta;
			if(!(anim.CurrentAnimation.Equals("Fall"))){
				anim.Play("Fall");
			}
			
		}

		if(velocity == Vector2.Zero && !(anim.CurrentAnimation.Equals("AtackSide")) && !(anim.CurrentAnimation.Equals("AtackUp")) && !(anim.CurrentAnimation.Equals("Crouch")) && !(anim.CurrentAnimation.Equals("Slide")) && !(anim.CurrentAnimation.Equals("Crouch"))){
			anim.Play("Idle");
		}

		Velocity = velocity;
		MoveAndSlide();	

		}

	public override void _Input(InputEvent @event){	

		direction = Input.GetVector("Izq", "Der", "ui_up", "ui_down");

		if(direction == Vector2.Left){
			Art.FlipH = true;
			velocity.X = -Speed;
			anim.Play("Run");
		}

		if(direction == Vector2.Right){
			Art.FlipH = false;
			velocity.X = Speed;
			anim.Play("Run");
		}


		if (direction == Vector2.Zero)
		{
			
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		}

		if(@event.IsActionPressed("Jump") && IsOnFloor()){
			velocity.Y = JumpVelocity;
			anim.Play("Jump");
			Velocity = velocity;
		}

		if(Input.IsActionPressed("Crouch") && IsOnFloor() && velocity == Vector2.Zero){
			if(!(anim.CurrentAnimation.Equals("Crouch"))){
				anim.Play("Crouch");
			}
		}

		if(@event.IsActionPressed("Atack")){
			anim.Play("AtackSide");
		}

		if(@event.IsActionPressed("Dash") && IsOnFloor()){
			anim.Play("Slide");
		}

		if(@event.IsActionPressed("Dash") && !IsOnFloor()){
			anim.Play("Dash");
		}

		if(@event.IsActionPressed("HardAtack")){
			anim.Play("AtackUp");
		}

		Velocity = velocity;

		
	}
}
