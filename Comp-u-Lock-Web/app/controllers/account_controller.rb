class AccountController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]
  load_and_authorize_resource
  # View
  def index
  	list
  	render("list")
  end
  # View
  def show
    @account = Account.find(params[:id])
  end
  # View
  def list
    @computers = current_user.computer.all
  end

  # form
  def edit
    @account = Account.find(params[:id])
  end

  
  # Put
  def update
    @account = Account.find(params[:id])
    if @account.update_attributes params[:account]
      flash[:notice] = "Information for your account has been updated."
      redirect_to action: 'edit', id: @account.id
    else
      flash[:notice] = "Something went wrong with saving account information."
      redirect_to action: 'edit', id: @account.id
    end
  end

  def destroy
    @account = Account.find(params[:id])
      if can? :destroy, @account
        @account.delete
      else
        flash[:notice] = "You can not delete this account."
      end
      redirect_to(:action => 'index')
  end

end
