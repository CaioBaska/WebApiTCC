namespace API_TCC.Repository
{
    public interface IServiceEnvioMqtt
    {
        void PublicarMensagem(string mensagem);
    }
}
