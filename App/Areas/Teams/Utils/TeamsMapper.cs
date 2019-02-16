using App.Areas.Teams.Models;
using App.Areas.Teams.ViewModels;
using AutoMapper;

namespace App.Areas.Teams.Utils
{
    public class TeamsMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            config.CreateMap<TeamViewModel, Team>();
            config.CreateMap<Team, TeamViewModel>();

            config.CreateMap<TeamMemberViewModel, TeamMember>();
            config.CreateMap<TeamMember, TeamMemberViewModel>();
        }
    }
}
