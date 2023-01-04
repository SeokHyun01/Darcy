using AutoMapper;
using Darcy_DataAccess;
using Darcy_DataAccess.ViewModel;
using Darcy_Models.Camera;
using Darcy_Models.Event;
using Darcy_Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darcy_Business.Mapper
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<Camera, CameraDTO>().ReverseMap();

			CreateMap<EventHeaderDTO, EventHeader>().ReverseMap();
			CreateMap<EventDetail, EventDetailDTO>().ReverseMap();
			CreateMap<EventDTO, Event>().ReverseMap();

			CreateMap<Post, PostDTO>().ReverseMap();
			CreateMap<Comment, CommentDTO>().ReverseMap();
		}
	}
}
