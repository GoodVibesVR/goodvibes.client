using GoodVibes.Client.ApiCaller.Requests;

namespace GoodVibes.Client.ApiCaller.Abstractions;

public interface IGoodVibesClient
{
    void SendCommand(CommandRequest request);
}