using MediCaseAPI.Models;

namespace MediCaseAPI.Seeders;

public record SampleNode(
    int TypeId,
    string TitlePl,
    string TitleEn,
    string? ContentPl = null,
    string? ContentEn = null,
    bool IsCorrect = false,
    SampleNode[]? Children = null);

public record SampleTerm(string TermPl, string DefinitionPl, string TermEn, string DefinitionEn);

/// <summary>
/// Example content shown during development and on the first presentation of the app.
/// </summary>
public static class SampleContent
{
    public static readonly SampleTerm[] AssessmentDictionary =
    {
        new("Duszność", "Subiektywne uczucie braku powietrza.",
            "Dyspnoea", "A subjective feeling of shortness of breath."),
        new("Saturacja", "Wysycenie hemoglobiny tlenem mierzone pulsoksymetrem.",
            "Oxygen saturation", "The share of haemoglobin carrying oxygen, measured with a pulse oximeter."),
        new("Omdlenie", "Krótkotrwała utrata przytomności z samoistnym powrotem.",
            "Syncope", "A short loss of consciousness with spontaneous recovery.")
    };

    public static readonly SampleNode[] Tree =
    {
        new(EntityTypeIds.Category, "Badanie pacjenta", "Patient assessment", Children: new SampleNode[]
        {
            new(EntityTypeIds.Content, "Schemat ABCDE", "ABCDE approach",
                "<p>Oceniaj pacjenta w stałej kolejności:</p><ol><li><strong>A</strong> – drożność dróg oddechowych</li><li><strong>B</strong> – oddychanie</li><li><strong>C</strong> – krążenie</li><li><strong>D</strong> – stan neurologiczny</li><li><strong>E</strong> – ekspozycja ciała</li></ol>",
                "<p>Assess the patient in a fixed order:</p><ol><li><strong>A</strong> – airway</li><li><strong>B</strong> – breathing</li><li><strong>C</strong> – circulation</li><li><strong>D</strong> – disability</li><li><strong>E</strong> – exposure</li></ol>"),
            new(EntityTypeIds.Content, "Wywiad SAMPLE", "SAMPLE history",
                "<p><strong>S</strong> – objawy, <strong>A</strong> – alergie, <strong>M</strong> – leki, <strong>P</strong> – przebyte choroby, <strong>L</strong> – ostatni posiłek, <strong>E</strong> – zdarzenia poprzedzające.</p>",
                "<p><strong>S</strong> – signs and symptoms, <strong>A</strong> – allergies, <strong>M</strong> – medications, <strong>P</strong> – past medical history, <strong>L</strong> – last oral intake, <strong>E</strong> – events leading up.</p>"),
            new(EntityTypeIds.QuestionList, "Rozmowa z pacjentem", "Talking to the patient", Children: new SampleNode[]
            {
                new(EntityTypeIds.Question, "Co się stało?", "What happened?",
                    "<p>Pytanie otwierające rozmowę.</p>", "<p>The opening question of the interview.</p>"),
                new(EntityTypeIds.Question, "Gdzie Pana boli?", "Where does it hurt?",
                    "<p>Poproś pacjenta, aby wskazał miejsce bólu.</p>", "<p>Ask the patient to point to the pain.</p>"),
                new(EntityTypeIds.Question, "Czy przyjmuje Pan jakieś leki?", "Do you take any medication?",
                    "<p>Zapytaj także o leki bez recepty.</p>", "<p>Also ask about over-the-counter drugs.</p>"),
                new(EntityTypeIds.Question, "Czy ma Pan na coś alergię?", "Are you allergic to anything?")
            }),
            new(EntityTypeIds.Test, "Test: badanie pacjenta", "Test: patient assessment", Children: new SampleNode[]
            {
                new(EntityTypeIds.TestQuestion, "Co oznacza litera A w schemacie ABCDE?",
                    "What does the letter A stand for in the ABCDE approach?", Children: new SampleNode[]
                    {
                        new(EntityTypeIds.TestAnswer, "Drogi oddechowe", "Airway", IsCorrect: true),
                        new(EntityTypeIds.TestAnswer, "Alergie", "Allergies"),
                        new(EntityTypeIds.TestAnswer, "Analgezja", "Analgesia")
                    }),
                new(EntityTypeIds.TestQuestion, "Co oznacza litera S w wywiadzie SAMPLE?",
                    "What does the letter S stand for in the SAMPLE history?", Children: new SampleNode[]
                    {
                        new(EntityTypeIds.TestAnswer, "Saturacja", "Saturation"),
                        new(EntityTypeIds.TestAnswer, "Objawy", "Signs and symptoms", IsCorrect: true),
                        new(EntityTypeIds.TestAnswer, "Stan świadomości", "State of consciousness")
                    })
            })
        }),
        new(EntityTypeIds.Category, "Resuscytacja", "Resuscitation", Children: new SampleNode[]
        {
            new(EntityTypeIds.Content, "Podstawowe zabiegi resuscytacyjne", "Basic life support",
                "<p>Sprawdź bezpieczeństwo, oceń przytomność i oddech, wezwij pomoc i rozpocznij uciskanie klatki piersiowej: 30 uciśnięć na 2 oddechy.</p>",
                "<p>Make sure the scene is safe, check responsiveness and breathing, call for help and start chest compressions: 30 compressions to 2 breaths.</p>")
        })
    };
}
