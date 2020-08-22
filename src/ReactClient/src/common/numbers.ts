
export function asCurrency(input: number): string {
    return new Intl.NumberFormat('en-IN', {
        style: 'currency',
        currency: 'USD'
      }).format(input);
}