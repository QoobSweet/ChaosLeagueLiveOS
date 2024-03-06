using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

// {get; set;} is required for dynamodb to automatically create playerprofile from entry

[Serializable]
[Table("PlayerProfiles")] 

public class PlayerProfile
{
    [PrimaryKey] 
    public string TwitchID { get; set; }
    public string TwitchUsername { get; set; }
    public string InvitedByID { get; set; }
    public string InvitesJSON { get; set; }

    public bool IsSubscriber { get; set; }

    public string NameColorHex { get; set; }
    public string CrownJSON {  get; set; }
    public string TrailGradientJSON { get; set; }
    public string SpeechBubbleFillHex { get; set; }
    public string SpeechBubbleTxtHex { get; set; }
    public string CurrentVoiceID { get; set; }
    [Ignore]
    public string[] PurchasedVoiceIDs { get; set; }

    // POINTS

    public int ThroneCaptures { get; set; }
    public int TimeOnThrone { get; set; }
    public int TotalTicketsSpent { get; set; }

    public int CurrentBid { get; set; }
   
    
    public int LifeTimeScore { get; set; }
    public int Gold { get; set; }
    public int SeasonScore { get; set; }
    public long SessionScore { get; set; }
    public long SessionScore_2 { get; set; }

    private var _ss1MonizationMap = new Dictionary<long, string>
    {
        {1000000000000000000, "Q"}, // quintillion
        {1000000000000000, "q"}, // quadrillion
        {1000000000000, "t"}, // trillion
        {1000000000, "b"}, // billion
        {1000000, "m"}, // million
        {1000, "k"}, // thousand
    };
    
    private var _ss2MonizationMap = new Dictionary<long, string>
    {
        {1000000000000000, "U"}, // undecillion
        {1000000000000, "D"}, // decillion
        {1000000000, "N"}, // nonillion
        {1000000, "O"}, // octillion
        {1000, "S"}, // septillion
        {1, "s"}, // sextillion
    };

    public string GetSessionScoreString()
    {
        string SS1 = "";
        string SS2 = "";

        if (SessionScore_2 > 0)
        {
            foreach (var pair in _ss2MonizationMap)
            {
                if (SessionScore_2 >= pair.Key)
                {
                    SS2 = (SessionScore_2 / pair.Key).ToString();
                    break;
                }
            }
        }
        else
        {
            foreach (var pair in _ss1MonizationMap)
            {
                if (SessionScore >= pair.Key)
                {
                    SS1 = (SessionScore / pair.Key).ToString();
                    break;
                }
            }
        }
        
        return SS2 != "" ? SS2 + GetSessionScoreAbbreviation() : SS1 + GetSessionScoreAbbreviation();
    }

    private string GetSessionScoreAbbreviation()
    {

        if (SessionScore_2 > 0)
        {
            foreach (var pair in _ss2MonizationMap)
            {
                if (SessionScore_2 >= pair.Key)
                {
                    return pair.Value;
                }
            }
        }
        else
        {
            foreach (var pair in _ss1MonizationMap)
            {
                if (SessionScore >= pair.Key)
                {
                    return pair.Value;
                }
            }
        }

        return "";
    }
    
    public DateTime LastInteraction { get; set; }

    public string[] GetInviteIds() 
    {
        if(string.IsNullOrEmpty(InvitesJSON))
            return Array.Empty<string>();
        return JsonConvert.DeserializeObject<string[]>(InvitesJSON);
    }

    private void SetInviteIds(string[] inviteIds)
    {
        InvitesJSON = JsonConvert.SerializeObject(inviteIds);
    }

    public void AddInvite(string id)
    {
        List<string> currentInvites = GetInviteIds().ToList();
        //Don't add if it's already in the list
        if (currentInvites.Contains(id))
            return;
        currentInvites.Add(id);
        SetInviteIds(currentInvites.ToArray());
    }
}