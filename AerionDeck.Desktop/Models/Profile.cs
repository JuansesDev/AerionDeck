using System;
using System.Collections.Generic;

namespace AerionDeck.Desktop.Models;

public class Profile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Default";
    public List<DeckAction> Actions { get; set; } = new();
}
