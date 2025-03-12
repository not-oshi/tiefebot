using DSharpPlus.Entities;

namespace tiefebot.Data.Function;

public class Dice
{
    public static Random rnd = new Random();

    public int D1 = rnd.Next(1, 7);
    public int D2 = rnd.Next(1, 7);
    public int DE = rnd.Next(1, 5);

    public bool IsAMess => (D1 == 1 && D2 == 6) || (D2 == 1 && D1 == 6);
    public bool IsCriticalSuccess => D1 + D2 == 12;
    public bool IsCriticalFailure => D1 + D2 == 2;
    public bool IsLightFortune => (D1 == 6 ^ D2 == 6);
    public bool IsLightUnfortune => (D1 == 1 ^ D2 == 1);
}

public class DiceCheck
{
    public static async Task<DiscordEmbed> CheckDice(long statNum)
    {
        Dice dice = new Dice();
        int result;
        DiscordEmbedBuilder embed;

        if (dice.IsAMess)
        {
            result = (int)statNum + dice.D1 + dice.D2;
            embed = new DiscordEmbedBuilder
            {
                Title = "A mess!",
                Description = $"{statNum} + {dice.D1} + {dice.D2} = {result}" 
            };
        }
        else if (dice.IsCriticalSuccess) 
        {
            result = (int)statNum + dice.D1 + dice.D2;
            embed = new DiscordEmbedBuilder
            {
                Title = "Critical Success!",
                Description = $"{statNum} + {dice.D1} + {dice.D2} = {result}"
            };
        }
        else if (dice.IsCriticalFailure) 
        {
            result = (int)statNum + dice.D1 + dice.D2;
            embed = new DiscordEmbedBuilder
            {
                Title = "Critical Failure!",
                Description = $"{statNum} + {dice.D1} + {dice.D2} = {result}"
            };
        }
        else if (dice.IsLightFortune) 
        {
            result = (int)statNum + dice.D1 + dice.D2 + dice.DE;
            embed = new DiscordEmbedBuilder
            {
                Title = "Light Fortune!",
                Description = $"{statNum} + {dice.D1} + {dice.D2} + {dice.DE} = {result}"
            };
        }
        else if (dice.IsLightUnfortune) 
        {
            result = (int)statNum + dice.D1 + dice.D2 - dice.DE;
            embed = new DiscordEmbedBuilder
            {
                Title = "Light Unfortune!",
                Description = $"{statNum} + {dice.D1} + {dice.D2} - {dice.DE} = {result}"
            };
        }
        else
        {
            //just normal check
            result = (int)statNum + dice.D1 + dice.D2;
            embed = new DiscordEmbedBuilder
            {
                Title = "Roll",
                Description = $"{statNum} + {dice.D1} + {dice.D2} = {result}"
            };
        }

        return embed;
    }
}