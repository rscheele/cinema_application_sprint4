using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class RoomRepository : IRoomRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Room> GetAllRooms()
        {
            return context.Rooms;
        }

        public Room GetRoom(int? RoomID)
        {
            Room room = context.Rooms.Where(x => x.RoomID == RoomID).FirstOrDefault();
            if (room != null)
            {
                return room;
            }
            else
            {
                return null;
            }
        }
    }
}
