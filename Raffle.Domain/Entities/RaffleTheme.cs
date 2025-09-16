using LeadSoft.Common.GlobalDomain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Raffle.Domain.Entities
{
    public class RaffleTheme : CollectionsBase
    {
        public string Id { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
        
        public string Category { get; set; } = string.Empty;
        
        public string PrimaryColor { get; set; } = "#9333ea";
        
        public string SecondaryColor { get; set; } = "#a855f7";
        
        public string AccentColor { get; set; } = "#ec4899";
        
        public string BackgroundColor { get; set; } = "#f8fafc";
        
        public string TextColor { get; set; } = "#1f2937";
        
        public string ButtonStyle { get; set; } = "rounded";
        
        public string FontFamily { get; set; } = "Inter";
        
        public string BackgroundImage { get; set; } = string.Empty;
        
        public string CustomCSS { get; set; } = string.Empty;
        
        public string TicketButtonText { get; set; } = "Escolher";
        
        public string PurchaseButtonText { get; set; } = "Comprar Números";
        
        public string HeaderTitle { get; set; } = "Participe da Rifa";
        
        public string FooterText { get; set; } = "Boa sorte!";
        
        public bool IsDefault { get; set; } = false;
        
        public bool IsActive { get; set; } = true;
    }
}