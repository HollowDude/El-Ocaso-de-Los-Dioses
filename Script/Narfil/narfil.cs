using Godot;
using System;

public partial class narfil : Node2D
{
    AnimationPlayer movs;
    AnimatedSprite2D Narf;
    players_glob ArtGlobs;
    Timer Back;
    bool yapuede = true;

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
       // GD.Print("Esta atacando:" + ArtGlobs.Action + "Tiene arma:" + ArtGlobs.WeaponOnHand);

        if(ArtGlobs.Action){

            movs.Play("Gone");
            yapuede = false;

        }

        if(ArtGlobs.WeaponOnHand && yapuede){
            Narf.Visible = false;
        }
        
        if(!(ArtGlobs.WeaponOnHand)){
            Narf.Visible = true;
            yapuede = true;
        }
        if(ArtGlobs.EnMovArt){
            if(ArtGlobs.DerArt){
                    Narf.FlipH = true;
                }else{
                    Narf.FlipH = false;
                }
            if(!(movs.CurrentAnimation.Equals("Follow")) && !(movs.CurrentAnimation.Equals("Back")) && yapuede){
                movs.Play("Back");
                Back.Start();
            }
        }
        else{
            if(!(movs.CurrentAnimation.Equals("Idle2")) && yapuede){
                movs.Play("Idle2");
            }
        }
    }
}



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
