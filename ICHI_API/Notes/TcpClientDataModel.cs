//namespace ICHI_API.Helpers
//{
//  public class TcpClientDataModel
//  {
//    /// <summary>
//    /// ID of message.
//    /// </summary>
//    public string MsgID { get; set; } = null!;

//    /// <summary>
//    /// ID of system.
//    /// </summary>
//    public string SystemId { get; set; } = null!;

//    /// <summary>
//    /// Number of message (Ex: 4E is 518).
//    /// </summary>
//    public string ProtocolNo { get; set; } = null!;

//    /// <summary>
//    /// Name of message (MelsecQnACpuFrame4E, MelsecQnACpuFrame3E, MelsecQnACpuFrameSLMP, MasterData).
//    /// </summary>
//    public string ProtocolName { get; set; } = null!;

//    /// <summary>
//    /// Type of Message (DataToFrameDLL, MemmoryCache).
//    /// </summary>
//    public string MsgType { get; set; } = null!;

//    /// <summary>
//    /// Frame Protocol information.
//    /// </summary>
//    public ProtocolDetail Protocol { get; set; } = new ProtocolDetail();

//    /// <summary>
//    /// Content of message.
//    /// </summary>
//    public string TcpMsgContent { get; set; } = null!;

//    public static TcpClientDataModel GennerateDataMemmoryCache()
//    {
//      TcpClientDataModel result = new TcpClientDataModel();
//      result.MsgID = "1";
//      result.SystemId = SystemKey.PCS;
//      result.ProtocolNo = "518";
//      result.ProtocolName = "MelsecQnACpuFrame4E";
//      result.MsgType = MessageType.MemmoryCache;

//      ProtocolDetail protDetail = new ProtocolDetail();
//      protDetail.NetworkNo = "00";
//      protDetail.PcNo = "FF";
//      protDetail.CpuWatchTimer = "10";
//      protDetail.PlcAccessIpAddress = "192.168.100.153";
//      protDetail.PlcAccessProt = "10000";
//      protDetail.RecvTimeOut = "3000";
//      protDetail.RetryCount = "3";
//      protDetail.TcpRecvWaitTime = "300";
//      protDetail.PingWaitTime = "3000";
//      protDetail.PingOpenCheck = "0";
//      protDetail.PingRWCheck = "0";
//      protDetail.AutomaticReturn = "1";
//      protDetail.Kyokuban = "00";
//      protDetail.DemamndUnitIONo = "03FF";
//      protDetail.CpuType = "Q";
//      protDetail.SirialNoNgWaitTime = "0";

//      result.Protocol = protDetail;

//      return result;
//    }
//  }

//  public class ProtocolDetail
//  {
//    public string NetworkNo { get; set; } = null!;

//    public string PcNo { get; set; } = null!;

//    public string CpuWatchTimer { get; set; } = null!;

//    public string PlcAccessIpAddress { get; set; } = null!;

//    public string PlcAccessProt { get; set; } = null!;

//    public string RecvTimeOut { get; set; } = null!;

//    public string RetryCount { get; set; } = null!;

//    public string TcpRecvWaitTime { get; set; } = null!;

//    public string PingWaitTime { get; set; } = null!;

//    public string PingOpenCheck { get; set; } = null!;

//    public string PingRWCheck { get; set; } = null!;

//    public string AutomaticReturn { get; set; } = null!;

//    public string Kyokuban { get; set; } = null!;

//    public string DemamndUnitIONo { get; set; } = null!;

//    public string CpuType { get; set; } = null!;

//    public string SirialNoNgWaitTime { get; set; } = null!;
//  }
//#pragma warning restore SA1600, SA1402
//}
