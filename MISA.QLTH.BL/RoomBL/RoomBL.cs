using MISA.QLTH.BL.BaseBL;
using MISA.QLTH.Common.Entities;
using MISA.QLTH.DL.RoomDL;

namespace MISA.QLTH.BL.RoomBL;

public class RoomBL : BaseBL<Room>, IRoomBL
{
    private readonly IRoomDL _roomDL;

    public RoomBL(IRoomDL roomDL) : base(roomDL)
    {
        _roomDL = roomDL;
    }
}