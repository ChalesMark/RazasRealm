public interface IInteractable
{
    
    bool AutoInteract { get; set; }
    void Interact(InteractController interactController);
    string GetInteractText();
} 
