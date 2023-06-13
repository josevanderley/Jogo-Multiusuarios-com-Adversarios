using System;
using System.Collections.Generic;
using System.Linq;

enum TipoCarta
{
    Gol,
    Penalti,
    Falta,
    CartaoVermelho,
    CartaoAmarelo,
    Energia
}

class Carta
{
    public TipoCarta Tipo { get; }

    public Carta(TipoCarta tipo)
    {
        Tipo = tipo;
    }
}

class Jogador
{
    public string Nome { get; }
    public int Gols { get; private set; }
    public int Pontos { get; private set; }
    public int Energia { get; private set; }

    public Jogador(string nome)
    {
        Nome = nome;
        Gols = 0;
        Pontos = 0;
        Energia = 10;
    }

    public void Jogar()
    {
        Console.WriteLine($"{Nome}, é a sua vez de jogar!");

        Console.WriteLine("Pressione Enter para abrir suas cartas...");
        Console.ReadLine();

        List<Carta> cartas = ObterCartasAleatorias();

        Console.Clear();
        Console.WriteLine("Suas cartas são:");
        ExibirCartas(cartas);

        if (TodasCartasIguais(cartas, TipoCarta.Gol))
        {
            Console.WriteLine("Três cartas de Gol! Você marcou um gol. Passa a vez para o adversário.");
            Gols++;
            return;
        }

        if (TodasCartasIguais(cartas, TipoCarta.Energia))
        {
            Console.WriteLine("Três cartas de Energia! Você ganhou mais uma energia. Passa a vez para o adversário.");
            Energia++;
            return;
        }

        if (TodasCartasIguais(cartas, TipoCarta.Penalti))
        {
            CobrarPenalti();
            return;
        }

        if (TodasCartasIguais(cartas, TipoCarta.Falta))
        {
            Console.WriteLine("Três cartas de Falta! Passa a vez para o adversário.");
            return;
        }

        if (TodasCartasIguais(cartas, TipoCarta.CartaoAmarelo))
        {
            Console.WriteLine("Três cartas de Cartão Amarelo! Você perdeu uma energia. Passa a vez para o adversário.");
            PerderEnergia(1);
            return;
        }

        if (TodasCartasIguais(cartas, TipoCarta.CartaoVermelho))
        {
            Console.WriteLine("Três cartas de Cartão Vermelho! Você perdeu duas energias. Passa a vez para o adversário.");
            PerderEnergia(2);
            return;
        }

        int pontosRodada = CalcularPontosRodada(cartas);
        Console.WriteLine($"Pontos da rodada: {pontosRodada}. Passa a vez para o adversário.");

        Pontos += pontosRodada;
    }

    public List<Carta> ObterCartasAleatorias()
    {
        List<Carta> cartasDisponiveis = ObterCartasDisponiveis();
        List<Carta> cartasAleatorias = new List<Carta>();
        Random random = new Random();

        while (cartasAleatorias.Count < 3 && cartasDisponiveis.Count > 0)
        {
            int indiceAleatorio = random.Next(0, cartasDisponiveis.Count);
            Carta cartaSelecionada = cartasDisponiveis[indiceAleatorio];
            cartasAleatorias.Add(cartaSelecionada);
            cartasDisponiveis.RemoveAt(indiceAleatorio);
        }

        return cartasAleatorias;
    }

    public List<Carta> ObterCartasDisponiveis()
    {
        List<Carta> cartasDisponiveis = new List<Carta>();

        for (int i = 1; i <= 6; i++)
        {
            TipoCarta tipoCarta = (TipoCarta)i;
            int quantidade = ObterQuantidadeCarta(tipoCarta);

            for (int j = 0; j < quantidade; j++)
            {
                Carta carta = new Carta(tipoCarta);
                cartasDisponiveis.Add(carta);
            }
        }

        return cartasDisponiveis;
    }

    public int ObterQuantidadeCarta(TipoCarta tipoCarta)
    {
        if (tipoCarta == TipoCarta.Gol)
        {
            return 1;
        }
        else if (tipoCarta == TipoCarta.Penalti)
        {
            return 2;
        }
        else if (tipoCarta == TipoCarta.Falta || tipoCarta == TipoCarta.CartaoAmarelo)
        {
            return 3;
        }
        else if (tipoCarta == TipoCarta.CartaoVermelho || tipoCarta == TipoCarta.Energia)
        {
            return 4;
        }

        return 0;
    }

    public bool TodasCartasIguais(List<Carta> cartas, TipoCarta tipoCarta)
    {
        return cartas.All(carta => carta.Tipo == tipoCarta);
    }

    public void CobrarPenalti()
    {
        Console.WriteLine("Três cartas de Pênalti! Escolha uma opção (Digite 1 para Direita, 2 para Esquerda ou 3 para Centro):");

        int opcao = LerOpcao(1, 3);

        Random random = new Random();
        int opcaoAdversario = random.Next(1, 4);

        if (opcao != opcaoAdversario)
        {
            Console.WriteLine("Gol! Você marcou um gol. Passa a vez para o adversário.");
            Gols++;
        }
        else
        {
            Console.WriteLine("Defendeu!!! Gol não contabilizado. Passa a vez para o adversário.");
        }
    }

    public void PerderEnergia(int quantidade)
    {
        Energia -= quantidade;

        if (Energia < 0)
        {
            Energia = 0;
        }
    }

    public int CalcularPontosRodada(List<Carta> cartas)
    {
        int pontos = 0;

        foreach (Carta carta in cartas)
        {
            if (carta.Tipo == TipoCarta.Gol)
            {
                pontos += 3;
            }
            else if (carta.Tipo == TipoCarta.Penalti)
            {
                pontos += 2;
            }
            else if (carta.Tipo == TipoCarta.Falta || carta.Tipo == TipoCarta.CartaoAmarelo)
            {
                pontos += 1;
            }
            else if (carta.Tipo == TipoCarta.Energia)
            {
                pontos += 2;
            }
        }

        return pontos;
    }

    public void ExibirCartas(List<Carta> cartas)
    {
        foreach (Carta carta in cartas)
        {
            Console.WriteLine(carta.Tipo.ToString());
        }
    }

    public int LerOpcao(int opcaoMinima, int opcaoMaxima)
    {
        int opcao;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= opcaoMinima && opcao <= opcaoMaxima)
            {
                break;
            }

            Console.WriteLine("Opção inválida! Digite novamente.");
        }

        return opcao;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Digite o nome do Jogador 1:");
        string nomeJogador1 = Console.ReadLine();

        Console.WriteLine("Digite o nome do Jogador 2:");
        string nomeJogador2 = Console.ReadLine();

        Jogador jogador1 = new Jogador(nomeJogador1);
        Jogador jogador2 = new Jogador(nomeJogador2);

        bool jogarNovamente = true;

        while (jogarNovamente)
        {
            Console.Clear();
            Console.WriteLine("----- Novo Jogo -----\n");

            while (jogador1.Energia > 0 && jogador2.Energia > 0)
            {
                jogador1.Jogar();

                if (jogador2.Energia > 0)
                {
                    jogador2.Jogar();
                }
            }

            ExibirResultado(jogador1, jogador2);

            Console.WriteLine("Digite o número '1' para sair ou '0' para jogar novamente:");
            int opcao = LerOpcao(0, 1);

            jogarNovamente = opcao == 0;
        }
    }

    public static void ExibirResultado(Jogador jogador1, Jogador jogador2)
    {
        Console.WriteLine("\n----- Resultado -----\n");
        Console.WriteLine($"Parabéns, {jogador1.Nome}! Você venceu a partida com {jogador1.Gols} gols e {jogador1.Pontos} pontos.");
        Console.WriteLine($"O seu adversário, {jogador2.Nome}, fez {jogador2.Gols} gols e {jogador2.Pontos} pontos.");
    }

    public static int LerOpcao(int opcaoMinima, int opcaoMaxima)
    {
        int opcao;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= opcaoMinima && opcao <= opcaoMaxima)
            {
                break;
            }

            Console.WriteLine("Opção inválida! Digite novamente.");
        }

        return opcao;
    }
}