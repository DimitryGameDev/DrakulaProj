
public class DeathNote : Notes
{
    protected override void CloseNotes()
    {
        base.CloseNotes();
        Character.Instance.GetComponent<Death>().DeathCharacter(2);
    }
}


 
