public interface IAttachmentReceiver
{
    bool HasAttachmentSocket(AttachmentType type);
    void ReceiveAttachment(AttachmentEffect effect);
}
