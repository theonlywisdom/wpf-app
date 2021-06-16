﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Friend
    {
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        public ProgrammingLanguage FavoriteLanguage { get; set; }

        public int? FavoriteLanguageId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
            Meetings = new Collection<Meeting>();
        }

        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
    }
}
