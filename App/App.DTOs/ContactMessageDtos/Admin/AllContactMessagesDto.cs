﻿namespace App.DTOs.ContactMessageDtos.Admin;
public class AllContactMessagesDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsRead { get; set; }
    public string? Reply { get; set; }
    public DateTime? ReplyDate { get; set; }
}