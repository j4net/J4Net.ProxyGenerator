namespace DSL
{
    public enum ModifierDescription
    {
        ABSTRACT,
        FINAL,
        NATIVE, // indicate method is implemented in native code
        PRIVATE,
        PROTECTED,
        PUBLIC,
        STATIC,
        SYNCHRONIZED, // multithreading, safely work 
        TRANSIENT, // exclude variable from serialization process
        VOLATILE // multithreading, each thread has copy variable
    }
}