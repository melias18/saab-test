using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using EmailService;

namespace TicketManagementSystem
{
    public class TicketService
    {
        public int CreateTicket(string title, Priority priority, string assignedTo, string description, DateTime datetime, bool isPayingCustomer)
        {
            
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                throw new InvalidTicketException("Title or description were null");
            }

            User user = FetchUser(assignedTo);
            priority = DeterminePriority(title, priority, datetime);
            Double price = CalculatePrice(isPayingCustomer, priority);

            var ticket = new Ticket()
            {
                Title = title,
                AssignedUser = user,
                Priority = priority,
                Description = description,
                Created = datetime,
                PriceDollars = price,
                AccountManager = isPayingCustomer ? new UserRepository().GetAccountManager() : null
            };

            var id = TicketRepository.CreateTicket(ticket);
            return id;
        }

        public void AssignTicket(int id, string username)
        {
            User user = FetchUser(username);
            var ticket = TicketRepository.GetTicket(id);
            
            if (ticket == null) throw new ApplicationException("No ticket found for id " + id);

            ticket.AssignedUser = user;
            TicketRepository.UpdateTicket(ticket);
        }

        private void WriteTicketToFile(Ticket ticket)
        {
            var ticketJson = JsonSerializer.Serialize(ticket);
            File.WriteAllText(Path.Combine(Path.GetTempPath(), $"ticket_{ticket.Id}.json"), ticketJson);
        }

        private User FetchUser(string username)
        {
            User user = null;
            using (var ur = new UserRepository())
            {
                user = assignedTo != null ? ur.GetUser(username) : null;
            }

            if (user == null) throw new UnknownUserException("User " + username + " not found");
            
            return user;
        }

        private Priority DeterminePriority(string title, Priority priority, DateTime datetime)
        {

            // Three things happen in this function and could/should be separated.
            var priorityRaised = false;
            if (datetime < DateTime.UtcNow - TimeSpan.FromHours(1))
            {
                priority = RaisePriority(priority);
                priorityRaised = true;
            }

            string[] badWords = {"Crash", "Important", "Failure"};
            if (title.Contains(badWords) && !priorityRaised)
            {
                RaisePriority(priority);
            }

            if (priority == Priority.High)
            {
                var emailService = new EmailServiceProxy();
                emailService.SendEmailToAdministrator(title, assignedTo);
            }

            return priority;
        }

        private Priority RaisePriority(Priority priority)
        {
            if (priority == Priority.High) return priority;
            if (priority == Priority.Low) return Priority.Medium;
            else if (priority == Priority.Medium) return Priority.High;
        }
    
        private Double CalculatePrice(bool isPayingCustomer, Priority priority)
        {
            double price = 0;
            if (isPayingCustomer)
            {
                price = priority ==  Priority.High ? 100 : 50;
            }

            return price;
        }
    }
    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
