using System;namespace MMI.EBuLa{
    public enum EntryType
    {
        OPS_MARKER=1, RADIO_MARKER, RADIO_MARKER_ENDING,
        GNT_BEGINNING, GNT_ENDING, 
        LZB_BEGINNING, LZB, LZB_ENDING,
        TUNNEL_BEGINNING, TUNNEL_ENDING,
        POSITION_JUMP, VERKUERTZT
    }
    public enum EntryPos
    {
        POS_STD=1, POS_GNT, SPEED_STD, SPEED_GNT,
        OPS_NAME, OPS_APP, POS, ETA, ETD, SL
    }
}
