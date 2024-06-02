namespace ROLAP.Parser.Enums;

internal enum TokenType
{
    /// <summary>
    /// Число
    /// </summary>
    Number,
    
    /// <summary>
    /// Идентификатор
    /// </summary>
    Identifier,

    /// <summary>
    /// [
    /// </summary>
    LBracket,
    
    /// <summary>
    /// ]
    /// </summary>
    RBracket,
    
    /// <summary>
    /// (
    /// </summary>
    LParent,
    
    /// <summary>
    /// )
    /// </summary>
    RParent,
    
    /// <summary>
    /// {
    /// </summary>
    LBrace,
    
    /// <summary>
    /// }
    /// </summary>
    RBrace,
    
    /// <summary>
    /// .
    /// </summary>
    Dot,
    
    /// <summary>
    /// &
    /// </summary>
    Ampersand,

    /// <summary>
    /// -
    /// </summary>
    Minus,
    
    /// <summary>
    /// +
    /// </summary>
    Plus,
    
    /// <summary>
    /// ,
    /// </summary>
    Comma,

    
    /// <summary>
    /// Разделитель
    /// </summary>
    Delimiter,
    
    /// <summary>
    /// Конец строки
    /// </summary>
    EOF,

    /// <summary>
    /// Select
    /// </summary>
    SELECT,
    
    /// <summary>
    /// ON
    /// </summary>
    ON,
    
    /// <summary>
    /// From
    /// </summary>
    FROM
}