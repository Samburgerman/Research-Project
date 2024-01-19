using System;

    private bool isGrounded = false;

    private void OnTriggerEnter(Collider other)
    {
        this.isGrounded = isGrounded;
    }
{
    private bool isGrounded;
    public GroundedData(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }
    public bool GetIsGrounded() { return isGrounded; }

    public override string ToString()
    { return ""+GetIsGrounded(); }
}