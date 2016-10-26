using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nala
{
    /// <summary>
    /// A wrapper for either a syntax node (<see cref="SyntaxNode"/>) or a syntax token (<see
    /// cref="SyntaxToken"/>).
    /// </summary>
    /// <remarks>
    /// Note that we do not store the token directly, we just store enough information to reconstruct it.
    /// This allows us to reuse nodeOrToken as a token's parent.
    /// </remarks>
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
    public struct SyntaxNodeOrToken : IEquatable<SyntaxNodeOrToken>
    {
        // In a case if we are wrapping a SyntaxNode this is the SyntaxNode itself.
        // In a case where we are wrapping a token, this is the token's parent.
        private readonly SyntaxNode _nodeOrParent;

        // Green node for the token. 
        private readonly SyntaxToken _token;
        
        internal SyntaxNodeOrToken(SyntaxNode node)
            : this()
        {
            if (node != null)
            {
                Debug.Assert(!node.IsList, "node cannot be a list");
                _nodeOrParent = node;
            }
            this.IsNode = true;
        }

        internal SyntaxNodeOrToken(SyntaxToken token)
            : this()
        {
            Debug.Assert(token == null || token.IsToken, "token must be a token");

            _token = token;
            this.IsNode = false;
        }

        internal SyntaxNode UnderlyingNode => _token ?? _nodeOrParent;

        internal string GetDebuggerDisplay()
        {
            return GetType().Name + " " + KindText + " " + ToString();
        }

        private string KindText
        {
            get
            {
                if (_token != null)
                {
                    return _token.KindText;
                }

                if (_nodeOrParent != null)
                {
                    return _nodeOrParent.KindText;
                }

                return "None";
            }
        }
        
        /// <summary>
        /// Determines whether this <see cref="SyntaxNodeOrToken"/> is wrapping a token.
        /// </summary>
        public bool IsToken => !IsNode;

        /// <summary>
        /// Determines whether this <see cref="SyntaxNodeOrToken"/> is wrapping a node.
        /// </summary>
        public bool IsNode { get; }

        /// <summary>
        /// Returns the underlying token if this <see cref="SyntaxNodeOrToken"/> is wrapping a
        /// token.
        /// </summary>
        /// <returns>
        /// The underlying token if this <see cref="SyntaxNodeOrToken"/> is wrapping a token.
        /// </returns>
        public SyntaxToken AsToken()
        {
            if (_token != null)
            {
                return _token;
            }

            return default(SyntaxToken);
        }

        /// <summary>
        /// Returns the underlying node if this <see cref="SyntaxNodeOrToken"/> is wrapping a
        /// node.
        /// </summary>
        /// <returns>
        /// The underlying node if this <see cref="SyntaxNodeOrToken"/> is wrapping a node.
        /// </returns>
        public SyntaxNode AsNode()
        {
            if (_token != null)
            {
                return null;
            }

            return _nodeOrParent;
        }

        /// <summary>
        /// Returns the string representation of this node or token, not including its leading and trailing
        /// trivia.
        /// </summary>
        /// <returns>
        /// The string representation of this node or token, not including its leading and trailing trivia.
        /// </returns>
        /// <remarks>The length of the returned string is always the same as Span.Length</remarks>
        public override string ToString()
        {
            if (_token != null)
            {
                return _token.ToString();
            }

            if (_nodeOrParent != null)
            {
                return _nodeOrParent.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Determines whether the supplied <see cref="SyntaxNodeOrToken"/> is equal to this
        /// <see cref="SyntaxNodeOrToken"/>.
        /// </summary>
        public bool Equals(SyntaxNodeOrToken other)
        {
            return _nodeOrParent == other._nodeOrParent &&
                   _token == other._token &&
                   this.IsNode == other.IsNode;
        }

        /// <summary>
        /// Determines whether two <see cref="SyntaxNodeOrToken"/>s are equal.
        /// </summary>
        public static bool operator ==(SyntaxNodeOrToken left, SyntaxNodeOrToken right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="SyntaxNodeOrToken"/>s are unequal.
        /// </summary>
        public static bool operator !=(SyntaxNodeOrToken left, SyntaxNodeOrToken right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether the supplied <see cref="SyntaxNodeOrToken"/> is equal to this
        /// <see cref="SyntaxNodeOrToken"/>.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is SyntaxNodeOrToken && Equals((SyntaxNodeOrToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _nodeOrParent?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (_token != null ? _token.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsNode.GetHashCode();
                return hashCode;
            }
        }
        
        /// <summary>
        /// Returns a new <see cref="SyntaxNodeOrToken"/> that wraps the supplied token.
        /// </summary>
        /// <param name="token">The input token.</param>
        /// <returns>
        /// A <see cref="SyntaxNodeOrToken"/> that wraps the supplied token.
        /// </returns>
        public static implicit operator SyntaxNodeOrToken(SyntaxToken token)
        {
            return new SyntaxNodeOrToken(token);
        }

        /// <summary>
        /// Returns the underlying token wrapped by the supplied <see cref="SyntaxNodeOrToken"/>.
        /// </summary>
        /// <param name="nodeOrToken">
        /// The input <see cref="SyntaxNodeOrToken"/>.
        /// </param>
        /// <returns>
        /// The underlying token wrapped by the supplied <see cref="SyntaxNodeOrToken"/>.
        /// </returns>
        public static explicit operator SyntaxToken(SyntaxNodeOrToken nodeOrToken)
        {
            return nodeOrToken.AsToken();
        }

        /// <summary>
        /// Returns a new <see cref="SyntaxNodeOrToken"/> that wraps the supplied node.
        /// </summary>
        /// <param name="node">The input node.</param>
        /// <returns>
        /// A <see cref="SyntaxNodeOrToken"/> that wraps the supplied node.
        /// </returns>
        public static implicit operator SyntaxNodeOrToken(SyntaxNode node)
        {
            return new SyntaxNodeOrToken(node);
        }

        /// <summary>
        /// Returns the underlying node wrapped by the supplied <see cref="SyntaxNodeOrToken"/>.
        /// </summary>
        /// <param name="nodeOrToken">
        /// The input <see cref="SyntaxNodeOrToken"/>.
        /// </param>
        /// <returns>
        /// The underlying node wrapped by the supplied <see cref="SyntaxNodeOrToken"/>.
        /// </returns>
        public static explicit operator SyntaxNode(SyntaxNodeOrToken nodeOrToken)
        {
            return nodeOrToken.AsNode();
        }
    }
}