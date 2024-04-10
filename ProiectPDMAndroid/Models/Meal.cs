namespace ProiectPDM.Models
{
    public class Meal
    {
        public string MealType { get; set; }
        public string Date { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string IngredientsString
        {
            get
            {
                var ingredientsString = Ingredients != null
                    ? string.Join("\n", Ingredients.Select(ing =>
                        $"• {ing.Name}: Calories: {ing.Calories} kcal, Protein: {ing.Protein} g, Fat: {ing.Fat} g, Carbs: {ing.Carbohydrates} g, Sugar: {ing.Sugar} g, Sodium: {ing.Sodium} mg, Potassium: {ing.Potassium} mg, Cholesterol: {ing.Cholesterol} mg, Serving Size: {ing.ServingSize} g"))
                    : "No ingredients";
                return ingredientsString;
            }
        }

    }
}
