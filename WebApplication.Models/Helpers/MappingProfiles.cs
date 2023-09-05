using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.DTOs;

namespace WebApp.Models.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() {
            CreateMap<RegisterDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<ReservationDTO, Reservation>();
            CreateMap<Residence, ResidenceDTO>();

        }
    }
}
