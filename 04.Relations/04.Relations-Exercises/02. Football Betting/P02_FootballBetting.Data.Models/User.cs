﻿namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;

using Common;

public class User
{
    public User()
    {
        this.Bets = new HashSet<Bet>();  
    }

    [Key]
    public int UserId { get; set; }

    [MaxLength(ValidationConstants.UserUsernameMaxLenght)]
    public string Username { get; set; } = null!;

    [MaxLength(ValidationConstants.UserPasswordMaxLenght)]
    public string Password { get; set; } = null!;

    [MaxLength(ValidationConstants.UserEmailMaxLenght)]
    public string Email { get; set; } = null!;

    [MaxLength(ValidationConstants.UserNameMaxLenght)]
    public string Name { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }

}
