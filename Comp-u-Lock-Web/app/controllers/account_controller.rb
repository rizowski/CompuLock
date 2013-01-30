class AccountController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]
  def index

  end

  def edit
  end

  def list

  end

  def update

  end

  def save

  end

  def show

  end
end
