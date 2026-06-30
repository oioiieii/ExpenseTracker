import { useCallback, useEffect, useState } from "react"
import { deleteExpense, getExpenses } from "@/api/expensesApi"
import { deleteCategory, getCategories } from "@/api/categoriesApi"
import CategoryForm from "@/components/CategoryForm"
import CategoryList from "@/components/CategoryList"
import ExpenseFilters from "@/components/ExpenseFilters"
import ExpenseForm from "@/components/ExpenseForm"
import ExpenseList from "@/components/ExpenseList"
import ExpenseStats from "@/components/ExpenseStats"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

function App() {
  const [expenses, setExpenses] = useState([])
  const [categories, setCategories] = useState([])
  const [editingExpense, setEditingExpense] = useState(null)
  const [editingCategory, setEditingCategory] = useState(null)
  const [filters, setFilters] = useState({
    dateFrom: "",
    dateTo: "",
    categoryId: "",
    search: "",
  })
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState(null)
  const [statsRefreshKey, setStatsRefreshKey] = useState(0)


  const loadExpenses = useCallback(async () => {
    try {
      setIsLoading(true)
      setError(null)

      const expensesData = await getExpenses(filters)
      setExpenses(expensesData)
    } catch (error) {
      setError(error.message)
    } finally {
      setIsLoading(false)
    }
  }, [filters])

  const loadCategories = useCallback(async () => {
    try {
      setError(null)

      const categoriesData = await getCategories()
      setCategories(categoriesData)
    } catch (error) {
      setError(error.message)
    }
  }, [])

  useEffect(() => {
    loadExpenses()
  }, [loadExpenses])

  useEffect(() => {
    loadCategories()
  }, [loadCategories])

  async function handleDeleteExpense(id) {
    try {
      setError(null)
      await deleteExpense(id)
      setEditingExpense((currentExpense) => (
        currentExpense?.id === id ? null : currentExpense
      ))
      await loadExpenses()
      setStatsRefreshKey((currentKey) => currentKey + 1)
    } catch (error) {
      setError(error.message)
    }
  }

  async function handleDeleteCategory(id) {
    try {
      setError(null)
      await deleteCategory(id)
      setEditingCategory((currentCategory) => (
        currentCategory?.id === id ? null : currentCategory
      ))
      await loadCategories()
      await loadExpenses()
      setStatsRefreshKey((currentKey) => currentKey + 1)
    } catch (error) {
      setError(error.message)
    }
  }

  async function handleExpenseSaved() {
    await loadExpenses()
    setEditingExpense(null)
    setStatsRefreshKey((currentKey) => currentKey + 1)
  }

  async function handleCategorySaved({ isEditing } = {}) {
    await loadCategories()
    setEditingCategory(null)

    if (isEditing) {
      setStatsRefreshKey((currentKey) => currentKey + 1)
    }
  }

  return (
    <div className="container mx-auto p-8">
      <h1 className="mb-6 text-2xl font-bold">Трекер расходов</h1>
      <main>
        <Tabs defaultValue="expenses" onValueChange={() => {
            setError(null)
        }}>
          <TabsList variant="line" className="mb-2">
            <TabsTrigger value="expenses">Расходы</TabsTrigger>
            <TabsTrigger value="categories">Категории</TabsTrigger>
            <TabsTrigger value="stats">Статистика</TabsTrigger>
          </TabsList>

          <TabsContent value="expenses">
            <div className="grid gap-6 lg:grid-cols-[1fr_380px] min-h-3/4">
              <div>
                <ExpenseFilters
                  categories={categories}
                  onApplyFilters={setFilters}
                />
                {isLoading && <p>Загрузка...</p>}
                {error && <p className="text-red-500">{error}</p>}
                {!isLoading && !error && (
                  <ExpenseList
                    expenses={expenses}
                    categories={categories}
                    onEditExpense={setEditingExpense}
                    onDeleteExpense={handleDeleteExpense}
                  />
                )}
              </div>

              <div className="space-y-6 lg:sticky lg:top-8 lg:self-start">
                <ExpenseForm
                  key={editingExpense?.id ?? "new-expense"}
                  categories={categories}
                  expense={editingExpense}
                  onExpenseSaved={handleExpenseSaved}
                  onCancelEdit={() => setEditingExpense(null)}
                />
              </div>
            </div>
          </TabsContent>

          <TabsContent value="categories">
            <div className="grid gap-6 lg:grid-cols-[1fr_380px]">
              <div>
                  <h2 className="text-lg font-semibold py-2 pl-4 pb-6">Список категорий</h2>
                {error && <p className="mb-4 text-red-500">{error}</p>}
                <CategoryList
                  categories={categories}
                  onEditCategory={setEditingCategory}
                  onDeleteCategory={handleDeleteCategory}
                />
              </div>

              <div className="space-y-6 lg:sticky lg:top-8 lg:self-start">
                <CategoryForm
                  key={editingCategory?.id ?? "new-category"}
                  category={editingCategory}
                  onCategorySaved={handleCategorySaved}
                  onCancelEdit={() => setEditingCategory(null)}
                />
              </div>
            </div>
          </TabsContent>

          <TabsContent value="stats">
            <ExpenseStats refreshKey={statsRefreshKey} />
          </TabsContent>
        </Tabs>
      </main>
    </div>
  )
}

export default App
