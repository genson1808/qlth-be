using MISA.QLTH.Common.Entities;
using QLTH.BL.BaseBL;
using QLTH.DL.RoomDL;

namespace QLTH.BL.RoomBL;

public class RoomBL : BaseBL<Room>, IRoomBL
{
    private readonly IRoomDL _roomDL;

    public RoomBL(IRoomDL roomDL) : base(roomDL)
    {
        _roomDL = roomDL;
    }
}