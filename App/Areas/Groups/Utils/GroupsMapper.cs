using App.Areas.Groups.Models;
using App.Areas.Groups.ViewModels;
using AutoMapper;

namespace App.Areas.Groups.Utils
{
    public class GroupsMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            config.CreateMap<GroupViewModel, Group>();
            config.CreateMap<Group, GroupViewModel>();

            config.CreateMap<MemberViewModel, Member>();
            config.CreateMap<Member, MemberViewModel>();
        }
    }
}
