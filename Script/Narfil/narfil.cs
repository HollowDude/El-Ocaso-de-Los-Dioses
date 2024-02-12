using Godot;
using System;

public partial class narfil : Node2D
{
    AnimationPlayer movs;
    AnimatedSprite2D Narf;
    players_glob ArtGlobs;
    Timer Back;

    public override void _Ready()
    {
       ArtGlobs = GetNode<players_glob>("/root/PlayersGlob");
       movs = GetNode("Movs") as AnimationPlayer;
       Narf = GetNode("Following/FollowingPath/Sprite") as AnimatedSprite2D;
       Back = GetNode<Timer>("Back");
       Back.Timeout += () => movs.Play("Follow");
    }

    public override void _PhysicsProcess(double delta)
    {
        if(ArtGlobs.EnMovArt == true){
            if(!(movs.CurrentAnimation.Equals("Follow")) && !(movs.CurrentAnimation.Equals("Back"))){
                if(ArtGlobs.DerArt){
                    Narf.FlipH = true;
                }else{
                    Narf.FlipH = false;
                }
                movs.Play("Back");
                Back.Start();
            }
        }
        else{
            if(!(movs.CurrentAnimation.Equals("Idle2"))){
                movs.Play("Idle2");
            }
        }
    }
}
