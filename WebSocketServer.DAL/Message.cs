using System;
using System.Collections.Generic;

namespace WebSocketServer.DAL;

public partial class Message
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public string? DateTime { get; set; }

    public string? ConnectionId { get; set; }
}
